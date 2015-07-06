using EnCoOrszag.Models.DataAccess.Entities;
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
                    //TODO: ez a foreach biztosan kivalthato egy SQL-el!

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

        public bool startResearch(string researchName)
        {
            using (var context = new ApplicationDbContext()) { 
                int activeCountryId = context.Users.First(c => c.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;

                bool canResearch = context.Researching.Count(
                    m => m.Country.Id == activeCountryId) < MAX_PARALLEL_RESEARCHES;

                if (canResearch)
                {
                    Researching researchStarted = new Researching();
                    researchStarted.Technology = context.Technologies.First(m => m.Name == researchName);

                    researchStarted.Country = context.Countries.First(
                        m => m.Id == activeCountryId
                    );

                    researchStarted.FinishTurn = context.Game.First(m => m.Id == 1).Turn + researchStarted.Technology.ResearchTime;
                    context.Researching.Add(researchStarted);
                    context.SaveChanges();
                    return true;
                }

                return false;
            }
        }


        public void endTurn()
        {
            using(var context = new ApplicationDbContext()){
                List<Country> countries = context.Countries.ToList();
                int turn = context.Game.First(m => m.Id == 1).Turn + 1;
                context.Game.First(m => m.Id == 1).Turn = turn;
                countries = payTaxesAndPotato(countries);
                //countries = dealWithArmy();
                countries = finishResearchs(countries, turn);
                countries = finishBuildings(countries, turn);
                context.Constructions.RemoveRange(context.Constructions.Where(m => m.Country == null));
                context.Researching.RemoveRange(context.Researching.Where(m => m.Country == null));
                
                context.SaveChanges();
            }
            
            //finishConstruction();
            //finishResearch();
        }

        public List<Country> payTaxesAndPotato(List<Country> countries)
        {
            foreach (var c in countries)
            {
                if (c.Buildings.Count(m => m.Blueprint.Name == "Cottage") > 0)
                {
                    int noCottages = c.Buildings.First(i => i.Blueprint.Name == "Cottage").NumberOfBuildings;
                    int tax = noCottages * 50 * 25;
                    c.Score += noCottages * 50;
                    int potato = noCottages * 200;
                    float goldBonuses = 1;
                    float potatoBonus = 1;
                    Research temp = c.Researches.FirstOrDefault(m => m.Technology.Name == "Tractor");
                    if (temp != null && temp.Finished) potatoBonus += 0.1f;
                    temp = c.Researches.FirstOrDefault(m => m.Technology.Name == "Harvester");
                    if (temp != null && temp.Finished) potatoBonus += 0.15f;
                    temp = c.Researches.FirstOrDefault(m => m.Technology.Name == "Alchemy");
                    if (temp != null && temp.Finished) goldBonuses += 0.3f;
                    tax = (int)(tax * goldBonuses);
                    potato = (int)(potato * potatoBonus);
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
                        c.Score += rs.Technology.Score;
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
                        
                        c.Score += 50;
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
    }
}