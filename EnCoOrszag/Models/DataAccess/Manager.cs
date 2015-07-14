﻿using EnCoOrszag.Models.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



using EnCoOrszag.ViewModell;

namespace EnCoOrszag.Models.DataAccess
{
    public class Manager
    {
        public readonly int MAX_PARALLEL_CONSTRUCTIONS = 4;
        public readonly int MAX_PARALLEL_RESEARCHES = 3;

        public bool isLogedIn()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return true;
            }
            return false;
        }

        public List<BuildingViewModel> makeBuildingViewModel()
        {
            using (var context = new ApplicationDbContext())
            {
                List<Blueprints> bls = context.Blueprints.ToList<Blueprints>();
                 int activeCountryId = context.Users.First(c => c.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;
                List<Building> buildingList = context.Buildings.Where(m => m.Country.Id == activeCountryId).ToList<Building>();

                List<BuildingViewModel> vmBls = new List<BuildingViewModel>();

                foreach (var item in bls)
                {
                    BuildingViewModel vmTemp = new BuildingViewModel();
                    vmTemp.Name = item.Name;
                    vmTemp.BuildTime = item.BuildTime;
                    vmTemp.Cost = item.Cost;
                    vmTemp.Description = item.Description;
                    vmTemp.Repeatable = item.Repeatable;
                    foreach (var building in buildingList)
                    {
                        if(building.Blueprint == item){
                            vmTemp.NoOfFinishedBlueprints = building.NumberOfBuildings;
                            break;
                        }
                    }
                    
                    vmBls.Add(vmTemp);
                }

                return vmBls;
            }
        }
        
        public List<ResearchViewModel> makeResearchViewModel()
        {
            using (var context = new ApplicationDbContext())
            {
                List<ResearchViewModel> vmResearch = new List<ResearchViewModel>();

                List<Technology> techList = context.Technologies.ToList<Technology>();
                
                int activeCountryId = context.Users.First(c => c.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;
                foreach (var tech in techList)
                {
                    ResearchViewModel temp = new ResearchViewModel();
                    temp.Name = tech.Name;
                    temp.Description = tech.Description;

                    if (context.Researches.Count() > 0)
                    {
                         if(context.Researches.Where(
                            m => m.Country.Id == activeCountryId
                            ).FirstOrDefault(c => c.Technology.Id == tech.Id) != null)
                         {
                             temp.Researched = true;
                         }
                         else
                         {
                             temp.Researched = false;
                         }
                    }
                    else
                    {
                        temp.Researched = false;
                    }
                    vmResearch.Add(temp);
                }

                return vmResearch;
            }
        }
        

        public Country getNewCountry(RegisterViewModel model)
        {
            using (var context = new ApplicationDbContext())
            {
                Country country = new Country();
                country.Gold = 1000;
                country.Potato = 1000;
                country.Name = model.CountryName;
                context.Countries.Add(country);
                return country;
            }
        }
        

        public bool startConstruction(string buildingName)
        {
            using (var context = new ApplicationDbContext())
            {
                int activeCountryId = context.Users.First(c => c.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;

                bool canConstruct = context.Constructions.Count(
                    m => m.Country.Id == activeCountryId) < MAX_PARALLEL_CONSTRUCTIONS;

                if (canConstruct)
                {
                    Construction buildingStarted = new Construction();
                    buildingStarted.Blueprint = context.Blueprints.First(m => m.Name == buildingName);

                    buildingStarted.Country = context.Countries.First(
                        m => m.Id == activeCountryId);

                    buildingStarted.FinishTurn = context.Game.First(m => m.Id == 1).Turn + buildingStarted.Blueprint.BuildTime;
                    context.Constructions.Add(buildingStarted);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public string startResearch(string researchName)
        {
            using (var context = new ApplicationDbContext()) { 
                int activeCountryId = context.Users.First(c => c.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;

                bool canResearch = context.Researching.Count(
                    m => m.Country.Id == activeCountryId) < MAX_PARALLEL_RESEARCHES;

                bool doingResearch = context.Researching.Where(
                    m => m.Country.Id == activeCountryId).Count(k => k.Technology.Name == researchName) < 1;


                if (canResearch && doingResearch)
                {
                    Researching researchStarted = new Researching();
                    researchStarted.Technology = context.Technologies.First(m => m.Name == researchName);

                    researchStarted.Country = context.Countries.First(
                        m => m.Id == activeCountryId
                    );

                    researchStarted.FinishTurn = context.Game.First(m => m.Id == 1).Turn + researchStarted.Technology.ResearchTime;
                    context.Researching.Add(researchStarted);
                    context.SaveChanges();
                    return "Research started.";
                }
                if (!doingResearch) return "You can't make multiple researches of the same type.";
                return "You can't make more than "+MAX_PARALLEL_RESEARCHES+" researches at the same time.";
            }
        }


        public string endTurn()
        {
            using(var context = new ApplicationDbContext()){
                List<Country> countries = context.Countries.ToList();
                int turn = context.Game.First(m => m.Id == 1).Turn + 1;
                context.Game.First(m => m.Id == 1).Turn = turn;
                countries = payTaxesAndPotato(countries);
                countries = dealWithArmy(countries);
                countries = finishResearchs(countries, turn);
                countries = finishBuildings(countries, turn);

                context.Constructions.RemoveRange(context.Constructions.Where(m => m.Country == null));
                context.Researching.RemoveRange(context.Researching.Where(m => m.Country == null));

                countries = battle(countries);
                countries = armiesReturnHome(countries);

                context.Assaults.RemoveRange(context.Assaults);
                context.Forces.RemoveRange(context.Forces);

                countries = calculateHighScore(countries);

                context.SaveChanges();
                return globalFake;
            }
            
            //finishConstruction();
            //finishResearch();
        }

        public List<Country> calculateHighScore(List<Country> countries)
        {
            using (var context = new ApplicationDbContext())
            {
                foreach (var c in countries)
                {
                    // Calculate population
                    c.Score += c.Population;
                    //  Calculate buildings
                    foreach (var b in c.Buildings)
                    {
                        c.Score += b.NumberOfBuildings * b.Blueprint.Score;
                    }
                    // Calculate armies
                    foreach (var a in c.StandingForce)
                    {
                        c.Score += a.Size * a.Type.Score;
                    }
                    // Calculate researches
                    foreach (var r in c.Researches)
                    {
                        c.Score += r.Technology.Score;
                    }
                }
                return countries;
            }
        }

        public List<Country> armiesReturnHome(List<Country> countries)
        {
            using (var context = new ApplicationDbContext())
            {
                foreach (var c in countries)
                {
                    foreach (var a in c.Assaults)
                    {
                        foreach (var f in a.Forces)
                        {
                            if (c.StandingForce.FirstOrDefault(m => m.Type.Id == f.Type.Id) != null)
                            {
                                c.StandingForce.FirstOrDefault(m => m.Type.Id == f.Type.Id).Size += f.Size;
                            }
                            else
                            {
                                Army army = new Army();
                                army.Size = f.Size;
                                army.Type = f.Type;
                                c.StandingForce.Add(army);
                            }
                        }
                        a.Forces.Clear();
                    }
                    c.Assaults.Clear();
                }
                return countries;
            }
        }

        string globalFake = "";

        public List<Country> battle(List<Country> countries)
        {
            globalFake = "";
            using (var context = new ApplicationDbContext())
            {
                foreach (var c in countries)
                {
                    foreach (var a in c.Assaults)
                    {
                        Country aim = a.Target;
                        if (aim != c)
                        {
                            List<Army> standing = aim.StandingForce.ToList<Army>();

                            float attackBonus = 1;
                            if (c.Researches.FirstOrDefault(m => m.Technology.Name == "Operation Rebirth") != null) attackBonus += 0.2f;
                            if (c.Researches.FirstOrDefault(m => m.Technology.Name == "Tactics") != null) attackBonus += 0.1f;

                            float defenseBonus = 1;
                            if (aim.Researches.FirstOrDefault(m => m.Technology.Name == "City walls") != null) defenseBonus += 0.2f;
                            if (aim.Researches.FirstOrDefault(m => m.Technology.Name == "Tactics") != null) defenseBonus += 0.1f;

                            int attackPower = 0;
                            foreach (var f in a.Forces)
                            {
                                attackPower += f.Size * f.Type.Attack;
                            }

                            int defensivePower = 0;
                            foreach (var f in aim.StandingForce)
                            {
                                defensivePower += f.Size * f.Type.Defense;
                            }

                            attackPower = (int)(attackPower * attackBonus);
                            defensivePower = (int)(defensivePower * defenseBonus);

                            //TODO: this is not suposed to be here.
                            globalFake += "Assault from: " + c.Name + " to " + aim.Name + " with " + attackPower + " power against " + defensivePower;

                            if (attackPower > defensivePower)
                            {
                                c.Gold += aim.Gold / 2;
                                aim.Gold -= aim.Gold / 2;

                                c.Potato += aim.Potato / 2;
                                aim.Potato -= aim.Potato / 2;

                                foreach(var lost in aim.StandingForce.ToList<Army>()) lost.Size = (int)(lost.Size * 0.9f);
                            }
                            else
                            {
                                foreach (var lost in a.Forces.ToList<Force>()) lost.Size = (int)(lost.Size * 0.9f);
                            }
                        }
                    }
                }
                return countries;
            }
        }

        public List<Country> dealWithArmy(List<Country> countries)
        {
            using (var context = new ApplicationDbContext())
            {
                foreach (var c in countries)
                {
                    int gold = 0;
                    int potato = 0;
                    List<Army> armies = c.StandingForce.ToList<Army>();
                    foreach (var a in armies)
                    {
                        int size = a.Size;
                        gold += context.UnitTypes.First(m => m.Id == a.Type.Id).Payment * size;
                        potato += context.UnitTypes.First(m => m.Id == a.Type.Id).Upkeep * size;
                    }
                    foreach (var a in c.Assaults)
                    {
                        foreach (var f in a.Forces)
                        {
                            int size = f.Size;
                            gold += context.UnitTypes.First(m => m.Id == f.Type.Id).Payment * size;
                            potato += context.UnitTypes.First(m => m.Id == f.Type.Id).Upkeep * size;
                        }
                    }
                    c.Gold -= gold;
                    c.Potato -= potato;
                }
                return countries;
            }
        }

        public List<Country> payTaxesAndPotato(List<Country> countries)
        {
            foreach (var c in countries)
            {
                if (c.Buildings.Count(m => m.Blueprint.Name == "Cottage") > 0)
                {
                    int noCottages = c.Buildings.First(i => i.Blueprint.Name == "Cottage").NumberOfBuildings;
                    int tax = c.Population * 25;
                    int potato = noCottages * 200;
                    float goldBonuses = 1;
                    float potatoBonus = 1;

                    Research temp = c.Researches.FirstOrDefault(m => m.Technology.Name == "Tractor");
                    if (temp != null && temp.Finished) potatoBonus += 0.1f;

                    temp = c.Researches.FirstOrDefault(m => m.Technology.Name == "Harvester");
                    if (temp != null && temp.Finished) potatoBonus += 0.15f;

                    temp = c.Researches.FirstOrDefault(m => m.Technology.Name == "Alchemy");
                    if (temp != null && temp.Finished) goldBonuses += 0.3f;
                    
                    potato = (int)(potato * potatoBonus);
                    tax = (int)(tax * goldBonuses);
                    c.Gold += tax;
                    c.Potato += potato;
                }
            }
            return countries;
        }

        public List<Country> finishResearchs(List<Country> countries, int turn)
        {
            foreach (var c in countries)
            {
                if (c.Researching.Count > 0)
                {
                    foreach (var r in c.Researching.Where(m => m.FinishTurn == turn).ToList())
                    {
                        Research rs = new Research();
                        rs.Country = c;
                        rs.Technology = c.Researching.First(i => i.Technology.Id == r.Technology.Id).Technology;
                        rs.Finished = true;
                        c.Researches.Add(rs);
                        c.Researching.Remove(c.Researching.Single(m => m.Id == r.Id));
                    }
                }
            }
            return countries;
        }

        public List<Country> finishBuildings(List<Country> countries, int turn)
        {
            foreach (var c in countries)
            {
                if (c.Construction.Count > 0)
                {
                    foreach (var b in c.Construction.Where(m => m.FinishTurn == turn).ToList())
                    {
                        Building bs = c.Buildings.FirstOrDefault(k => k.Blueprint == b.Blueprint);
                        if(bs == null)
                        {
                            bs = new Building();
                            c.Buildings.Add(bs);
                        }
                        bs.Blueprint = b.Blueprint;
                        bs.Country = c;
                        bs.NumberOfBuildings += 1;
                        if (b.Blueprint.Name == "Cottage") c.Population += 50;
                        c.Construction.Remove(c.Construction.Single(m => m.Id == b.Id));
                    }
                }
            }
            return countries;
        }

        public List<ConstructionViewModel> makeConstructionViewModel()
        {
            using (var context = new ApplicationDbContext())
            {
                int activeCountryId = context.Users.First(c => c.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;
                List<ConstructionViewModel> vmConstList = new List<ConstructionViewModel>();
                List<Construction> constList = context.Constructions.Where(m => m.Country.Id == activeCountryId).ToList<Construction>();

                foreach (var item in constList)
                {
                    ConstructionViewModel element = new ConstructionViewModel();
                    element.Name = item.Blueprint.Name;
                    element.TurnsLeft = item.Blueprint.BuildTime + context.Game.First(m => m.Id == 1).Turn - item.FinishTurn ;
                    element.WholeTime = item.Blueprint.BuildTime;

                    element.Id = item.Id;

                    vmConstList.Add(element);
                }

                return vmConstList;
            }
        }

        public List<ResearchingViewModel> makeResearchingViewModel()
        {
            using (var context = new ApplicationDbContext())
            {
                int activeCountryId = context.Users.First(c => c.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;
                List<ResearchingViewModel> vmResearchList = new List<ResearchingViewModel>();
                List<Researching> researchList = new List<Researching>();

                researchList = context.Researching.Where(m => m.Country.Id == activeCountryId).ToList<Researching>();

                foreach (var item in researchList)
                {
                    ResearchingViewModel element = new ResearchingViewModel();
                    element.Name = item.Technology.Name;
                    element.TurnsLeft = item.Technology.ResearchTime + context.Game.First(m => m.Id == 1).Turn - item.FinishTurn;
                    element.WholeTime = item.Technology.ResearchTime;
                    element.Id = item.Id;
                    vmResearchList.Add(element);
                }
                return vmResearchList;
            }
        }

        public CountryViewModel makeCountryViewModel()
        {
            using (var context = new ApplicationDbContext())
            {
                int activeCountryId = context.Users.First(c => c.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;
                CountryViewModel vmCountry = new CountryViewModel();
                string uName = System.Web.HttpContext.Current.User.Identity.Name;
                Country country = context.Countries.First(c => c.Id == activeCountryId);
                vmCountry.Id = country.Id;
                vmCountry.Name = country.Name;
                vmCountry.Score = country.Score;
                vmCountry.Gold = country.Gold;
                vmCountry.Potato = country.Potato;
                vmCountry.Turn = context.Game.First(m => m.Id == 1).Turn;
                return vmCountry;
            }
        }

        public void cancelConstruction(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Constructions.Remove(context.Constructions.First(m => m.Id == id));
                context.SaveChanges();
            }
        }

        public void cancelResearch(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Researching.Remove(context.Researching.First(m => m.Id == id));
                context.SaveChanges();
            }
        }

        public List<HighScoreViewModel> makeHighScoreViewModel()
        {
            using (var context = new ApplicationDbContext())
            {
                List<HighScoreViewModel> vmH = new List<HighScoreViewModel>();
                foreach (var c in context.Countries.ToList())
                {
                    HighScoreViewModel hs = new HighScoreViewModel();
                    hs.Name = c.Name;
                    hs.Score = c.Score;
                    vmH.Add(hs);
                }
                vmH = vmH.OrderByDescending(m => m.Score).ToList();
                return vmH;
            }
        }

        public ArmyRecruitViewModel makeArmyRecruitViewModel()
        {
            using(var context = new ApplicationDbContext()){

                int activeCountryId = context.Users.First(c => c.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;

                ArmyRecruitViewModel vmAR = new ArmyRecruitViewModel();
                vmAR.Gold = context.Countries.First(m => m.Id == activeCountryId).Gold;
                vmAR.Potato = context.Countries.First(m => m.Id == activeCountryId).Potato;
                List<UnitType> unitList = context.UnitTypes.ToList<UnitType>();
                List<UnitTypeViewModel> vmUnit = new List<UnitTypeViewModel>();

                Country country = context.Countries.First(m => m.Id == activeCountryId);
                int sum = 0;
                foreach (var item in unitList)
                {
                    UnitTypeViewModel vmUT = new UnitTypeViewModel();
                    vmUT.Name = item.Name;
                    vmUT.Payment = item.Payment;
                    vmUT.Attack = item.Attack;
                    vmUT.Defense = item.Defense;
                    vmUT.Cost = item.Cost;
                    vmUT.Upkeep = item.Upkeep;
                    vmUT.Description = item.Description;
                    Army army = country.StandingForce.FirstOrDefault(m => m.Type.Id == item.Id);
                    if (army == null)
                        vmUT.Size = 0;
                    else
                        vmUT.Size = army.Size;
                    vmUT.Id = item.Id;
                    sum += vmUT.Size;
                    vmUnit.Add(vmUT);
                }
                vmAR.Types = vmUnit;
                vmAR.OccupiedSpace = sum;
                Building building = context.Buildings.Where(m => m.Country.Id == activeCountryId).FirstOrDefault(m => m.Blueprint.Name == "Barrack");
                if (building == null) sum = 0;
                else sum = building.NumberOfBuildings;
                sum = sum * 200;
                vmAR.AllSpace = sum;
                return vmAR;
            }
        }

        public string recruitTroops(int id, int amount)
        {
            if (amount > 0)
            {
                using (var context = new ApplicationDbContext())
                {
                    int activeCountryId = context.Users.First(c => c.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;

                    Country country = context.Countries.First(m => m.Id == activeCountryId);
                    List<Army> armies = country.StandingForce.ToList<Army>();
                    int armySize = 0;
                    foreach (var item in armies)
                    {
                        armySize += item.Size;
                    }
                    int cost = context.UnitTypes.First(m => m.Id == id).Cost;
                    bool enoughPlace = context.Buildings.Where(m => m.Country.Id == activeCountryId).First(m => m.Blueprint.Name == "Barrack").NumberOfBuildings*200 >=
                       (armySize + amount);

                    if (country.Gold >= cost * amount && enoughPlace)
                    {
                        Army standing = country.StandingForce.FirstOrDefault(m => m.Type == context.UnitTypes.First(k => k.Id == id));
                        UnitType ut = context.UnitTypes.First(m => m.Id == id);
                        if (standing == null)
                        {
                            standing = new Army();
                            standing.Type = ut;

                            standing.Size = amount;
                            country.StandingForce.Add(standing);
                        }
                        else
                        {
                            country.StandingForce.First(m => m.Type == ut).Size += amount;
                        }
                        country.Gold -= ut.Cost * amount;
                        context.SaveChanges();
                        return "You recruited the troops.";
                    }
                }
            }
            return "You can't recruit this many troops, make sure you have enough gold and/or living quarters.";
        }

        public AssaultViewModel makeAssaultViewModel()
        {
            using (var context = new ApplicationDbContext())
            {
                int activeCountryId = context.Users.First(c => c.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;
                AssaultViewModel vmAssault = new AssaultViewModel();

                vmAssault.Countries = context.Countries.ToList<Country>();

                Country country = context.Countries.First(m => m.Id == activeCountryId);

                List<Army> armies = new List<Army>();

                foreach (var item in country.StandingForce.ToList<Army>())
                {
                    Army army = new Army();
                    army.Size = item.Size;
                    UnitType ut = new UnitType();
                    ut.Name = item.Type.Name;
                    ut.Attack = item.Type.Attack;
                    ut.Defense = item.Type.Defense;
                    ut.Description = item.Type.Description;
                    army.Type = ut;
                    armies.Add(army);
                }

                
               // vmAssault.Armies = country.StandingForce.ToList<Army>();
                vmAssault.Armies = armies;

                AssaultsCollectModel cmAssault = new AssaultsCollectModel();


                return vmAssault;
            }
        }
    }
}