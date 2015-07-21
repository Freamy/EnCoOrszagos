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
        public static readonly int MAX_PARALLEL_CONSTRUCTIONS = 4;
        public static readonly int MAX_PARALLEL_RESEARCHES = 3;

        //TODO: A bemeneti ertekek stringkent vannak kezelve, negativ ertekek
        //TODO: Recruit oldalon hány katonád van
        //TODO: Kapj forcest alapbol.

        public static bool IsLogedIn()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return true;
            }
            return false;
        }

        public static List<BuildingViewModel> MakeBuildingViewModel()
        {
            using (var context = new ApplicationDbContext())
            {
                int activeCountryId = context.Users.First(c => c.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;
                List<BuildingViewModel> vmBls = new List<BuildingViewModel>();


                vmBls = context.Blueprints.Select(m => new BuildingViewModel()
                {
                    Name = m.Name,
                    Cost = m.Cost,
                    Description = m.Description,
                    Repeatable = m.Repeatable,
                    BuildTime = m.BuildTime,
                    NoOfFinishedBlueprints = context.Buildings.Where(k => k.Country.Id == activeCountryId)
                     .FirstOrDefault(k => k.Blueprint.Name == m.Name) != null ? context.Buildings
                     .FirstOrDefault(k => k.Blueprint.Name == m.Name).NumberOfBuildings : 0
                }).ToList();

                return vmBls;
            }
        }

        public static List<ResearchViewModel> MakeResearchViewModel()
        {
            using (var context = new ApplicationDbContext())
            {
                List<ResearchViewModel> vmResearch = new List<ResearchViewModel>();
               
                int activeCountryId = context.Users.First(c => c.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;

                vmResearch = context.Technologies.Select(m => new ResearchViewModel()
                {
                    Name = m.Name,
                    Description = m.Description,
                    Researched = context.Researches.Where(k => k.Country.Id == activeCountryId).FirstOrDefault(k => k.Technology.Id == m.Id) != null 
                    ? true : false
                }).ToList();
                 return vmResearch;
            }
        }


        public static Country GetNewCountry(RegisterViewModel model)
        {
            using (var context = new ApplicationDbContext())
            {
                Country country = new Country()
                {
                    Gold = 10000,
                    Potato = 10000,
                    Name = model.CountryName,
                };
                context.Countries.Add(country);
                return country;
            }
        }


        public static bool StartConstruction(string buildingName)
        {
            using (var context = new ApplicationDbContext())
            {
                int activeCountryId = context.Users.First(c => c.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;

                bool canConstruct = context.Constructions.Count(m => m.Country.Id == activeCountryId) < MAX_PARALLEL_CONSTRUCTIONS;

                if (canConstruct)
                {
                    Construction buildingStarted = new Construction()
                    {
                        Blueprint = context.Blueprints.First(m => m.Name == buildingName),
                        Country = context.Countries.First(m => m.Id == activeCountryId),
                        FinishTurn = context.Game.First(m => m.Id == 1).Turn + context.Blueprints.First(m => m.Name == buildingName).BuildTime
                    };
                    context.Constructions.Add(buildingStarted);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public static string StartResearch(string researchName)
        {
            using (var context = new ApplicationDbContext()) { 
                int activeCountryId = context.Users.First(c => c.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;

                bool canResearch = context.Researching.Count(m => m.Country.Id == activeCountryId) < MAX_PARALLEL_RESEARCHES;

                bool doingResearch = context.Researching.Where(m => m.Country.Id == activeCountryId)
                    .Count(k => k.Technology.Name == researchName) < 1;


                if (canResearch && doingResearch)
                {
                    Researching researchStarted = new Researching()
                    {
                        Technology = context.Technologies.First(m => m.Name == researchName),
                        Country = context.Countries.First(m => m.Id == activeCountryId),
                        FinishTurn = context.Game.First(m => m.Id == 1).Turn + context.Technologies.First(m => m.Name == researchName).ResearchTime
                    };


                    context.Researching.Add(researchStarted);
                    context.SaveChanges();
                    return "Research started.";
                }
                if (!doingResearch) return "You can't make multiple researches of the same type.";
                return "You can't make more than "+MAX_PARALLEL_RESEARCHES+" researches at the same time.";
            }
        }


        public static void EndTurn()
        {
            using(var context = new ApplicationDbContext()){
                List<Country> countries = context.Countries.ToList();
                int turn = context.Game.First(m => m.Id == 1).Turn + 1;
                context.Game.First(m => m.Id == 1).Turn = turn;
                countries = PayTaxesAndPotato(countries);
                countries = DealWithArmy(countries);
                countries = FinishResearchs(countries, turn);
                countries = FinishBuildings(countries, turn);

                context.Constructions.RemoveRange(context.Constructions.Where(m => m.Country == null));
                context.Researching.RemoveRange(context.Researching.Where(m => m.Country == null));

                countries = Battle(countries);
                countries = ArmiesReturnHome(countries);

                context.Assaults.RemoveRange(context.Assaults);
                context.Forces.RemoveRange(context.Forces);

                countries = DealWithStarvation(countries);

                countries = CalculateHighScore(countries);

                context.SaveChanges();
            }
            
            //finishConstruction();
            //finishResearch();
        }

        public static List<Country> CalculateHighScore(List<Country> countries)
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

  
        public static List<Country> ArmiesReturnHome(List<Country> countries)
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
                                c.StandingForce.First(m => m.Type.Id == f.Type.Id).Size += f.Size;
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


        public static List<Country> Battle(List<Country> countries)
        {
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

                            string result;
                            int wonPotato = 0;
                            int wonGold = 0;
                            string losses = "";

                            if (attackPower > defensivePower)
                            {
                                result = "Won";
                                wonPotato = aim.Potato / 2;
                                wonGold = aim.Gold / 2;
                            }
                            else if(attackPower == defensivePower)
                            {
                                result = "Tie";
                            }
                            else
                            {
                                result = "Lost";
                            }

                            

                            

                            if (attackPower > defensivePower)
                            {
                                c.Gold += aim.Gold / 2;
                                aim.Gold -= aim.Gold / 2;

                                c.Potato += aim.Potato / 2;
                                aim.Potato -= aim.Potato / 2;

                                foreach (var lost in aim.StandingForce.ToList<Army>())
                                {
                                    lost.Size = (int)(lost.Size * 0.9f);
                                }
                            }
                            else
                            {
                                foreach (var lost in a.Forces.ToList<Force>())
                                {
                                    int lostForces = (int)(lost.Size * 0.9f);
                                    lost.Size = (int)(lost.Size * 0.9f);
                                    losses += lost.Type.Name + " " + lostForces + " "; 
                                }
                            }

                            BattleHistory battle = new BattleHistory()
                            {

                                Attacker = c.Name,
                                Defender = aim.Name,
                                Result = result,
                                WonGold = wonPotato,
                                WonPotato = wonGold,
                                Losses = losses,
                                Attack = attackPower,
                                Defense = defensivePower,
                                Turn = context.Game.First(m => m.Id == 1).Turn
                            };
                            context.History.Add(battle);
                            context.SaveChanges();
                        }
                    }
                }
                return countries;
            }
        }

        public static List<Country> DealWithArmy(List<Country> countries)
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

        public static List<Country> PayTaxesAndPotato(List<Country> countries)
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

        public static List<Country> FinishResearchs(List<Country> countries, int turn)
        {
            foreach (var c in countries)
            {
                if (c.Researching.Count > 0)
                {
                    foreach (var r in c.Researching.Where(m => m.FinishTurn == turn).ToList())
                    {
                        Research rs = new Research()
                        {
                            Country = c,
                            Technology = c.Researching.FirstOrDefault(i => i.Technology.Id == r.Technology.Id).Technology,
                            Finished = true,
                        };
                        c.Researches.Add(rs);
                        c.Researching.Remove(c.Researching.Single(m => m.Id == r.Id));
                    }
                }
            }
            return countries;
        }

        public static List<Country> FinishBuildings(List<Country> countries, int turn)
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
                            bs = new Building()
                            {
                                Blueprint = b.Blueprint,
                                Country = b.Country,
                                NumberOfBuildings = 0
                            };
                            c.Buildings.Add(bs);
                        }
                        bs.NumberOfBuildings += 1;
                        if (b.Blueprint.Name == "Cottage") c.Population += 50;
                        c.Construction.Remove(c.Construction.Single(m => m.Id == b.Id));
                    }
                }
            }
            return countries;
        }

        public static List<Country> DealWithStarvation(List<Country> countries)
        {
            foreach(var c in countries){
                if (c.Potato < 0 || c.Gold < 0)
                {
                    foreach (var a in c.StandingForce)
                    {
                        a.Size -= a.Size / 2;
                    }
                }
                if (c.Potato < 0) c.Potato = 0;
                if (c.Gold < 0) c.Gold = 0;
            }
            return countries;
        }

        public static List<ConstructionViewModel> MakeConstructionViewModel()
        {
            using (var context = new ApplicationDbContext())
            {
                int activeCountryId = context.Users.First(c => c.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;
                List<ConstructionViewModel> vmConstList = new List<ConstructionViewModel>();
                List<Construction> constList = context.Constructions.Where(m => m.Country.Id == activeCountryId).ToList<Construction>();

                vmConstList = context.Constructions.Select(m => new ConstructionViewModel()
                {
                    Name = m.Blueprint.Name,
                    TurnsLeft = m.Blueprint.BuildTime + context.Game.FirstOrDefault(k => k.Id == 1).Turn - m.FinishTurn,
                    WholeTime = m.Blueprint.BuildTime,
                    Id = m.Id
                }).ToList();

                return vmConstList;
            }
        }

        public static List<ResearchingViewModel> MakeResearchingViewModel()
        {
            using (var context = new ApplicationDbContext())
            {
                int activeCountryId = context.Users.First(c => c.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;

                List<ResearchingViewModel> vmResearchList = context.Researching.Select(m => new ResearchingViewModel()
                {
                    Name = m.Technology.Name,
                    TurnsLeft = m.Technology.ResearchTime + context.Game.FirstOrDefault(k => k.Id == 1).Turn - m.FinishTurn,
                    WholeTime = m.Technology.ResearchTime,
                    Id = m.Id
                }).ToList();
  
                return vmResearchList;
            }
        }

        public static CountryViewModel MakeCountryViewModel()
        {
            using (var context = new ApplicationDbContext())
            {
                int activeCountryId = context.Users.First(c => c.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;
                CountryViewModel vmCountry = new CountryViewModel()
                {
                    Id = context.Countries.First(c => c.Id == activeCountryId).Id,
                    Name = context.Countries.First(c => c.Id == activeCountryId).Name,
                    Score = context.Countries.First(c => c.Id == activeCountryId).Score,
                    Gold = context.Countries.First(c => c.Id == activeCountryId).Gold,
                    Potato = context.Countries.First(c => c.Id == activeCountryId).Potato,
                    Turn = context.Game.FirstOrDefault(m => m.Id == 1).Turn
                };

                List<BattleResultViewModel> bhL = new List<BattleResultViewModel>();
                foreach(var item in context.History.Where(m => m.Turn == context.Game.FirstOrDefault(mm => mm.Id == 1).Turn-1))
                {
                    BattleResultViewModel bh = new BattleResultViewModel()
                    {
                        Attack = item.Attack,
                        Attacker = item.Attacker,
                        Defender = item.Defender,
                        Defense = item.Defense,
                        Losses = item.Losses,
                        Result = item.Result,
                        Turn = item.Turn,
                        WonGold = item.WonGold,
                        WonPotato = item.WonPotato
                    };
                    bhL.Add(bh);
                }
                vmCountry.History = bhL;
                
                return vmCountry;
            }
        }

        public static void CancelConstruction(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Constructions.Remove(context.Constructions.First(m => m.Id == id));
                context.SaveChanges();
            }
        }

        public static void CancelResearch(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Researching.Remove(context.Researching.First(m => m.Id == id));
                context.SaveChanges();
            }
        }

        public static List<HighScoreViewModel> MakeHighScoreViewModel()
        {
            using (var context = new ApplicationDbContext())
            {
                List<HighScoreViewModel> vmH = new List<HighScoreViewModel>();
                vmH = context.Countries.Select(m => new HighScoreViewModel()
                {
                    Name = m.Name,
                    Score = m.Score
                }).ToList();
                
                vmH = vmH.OrderByDescending(m => m.Score).ToList();
                return vmH;
            }
        }

        public static ArmyRecruitViewModel MakeArmyRecruitViewModel()
        {
            using(var context = new ApplicationDbContext())
            {

                int activeCountryId = context.Users.First(c => c.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;

                List<UnitType> unitList = context.UnitTypes.ToList<UnitType>();
                List<UnitTypeViewModel> vmUnit = new List<UnitTypeViewModel>();

                Country country = context.Countries.First(m => m.Id == activeCountryId);
                int sum = 0;
                vmUnit = context.UnitTypes.Select(m => new UnitTypeViewModel()
                {
                    Name = m.Name,
                    Payment = m.Payment,
                    Attack = m.Attack,
                    Defense = m.Defense,
                    Cost = m.Cost,
                    Upkeep = m.Upkeep,
                    Description = m.Description,
                    Id = m.Id,
                   // Size = country.StandingForce.FirstOrDefault(k => k.Type.Id == m.Id).Size
                }).ToList();

                sum = country.StandingForce.Sum(m => m.Size);
         
                ArmyRecruitViewModel vmAR = new ArmyRecruitViewModel()
                {
                    Types = vmUnit,
                    OccupiedSpace = sum,
                    AllSpace = context.Buildings.Where(m => m.Country.Id == activeCountryId).FirstOrDefault(m => m.Blueprint.Name == "Barrack") != null 
                    ? context.Buildings.Where(m => m.Country.Id == activeCountryId).FirstOrDefault(m => m.Blueprint.Name == "Barrack").NumberOfBuildings *200 : 0
                };
                vmAR.Gold = context.Countries.First(m => m.Id == activeCountryId).Gold;
                vmAR.Potato = context.Countries.First(m => m.Id == activeCountryId).Potato;
                return vmAR;
            }
        }

        public static string RecruitTroops(int id, int amount)
        {
            if (amount > 0)
            {
                using (var context = new ApplicationDbContext())
                {
                    int activeCountryId = context.Users.First(c => c.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;

                    Country country = context.Countries.First(m => m.Id == activeCountryId);
                    List<Army> armies = country.StandingForce.ToList<Army>();
                    int armySize = country.StandingForce.Sum(m => m.Size);
                    int cost = context.UnitTypes.First(m => m.Id == id).Cost;
                    bool enoughPlace = context.Buildings.Where(m => m.Country.Id == activeCountryId)
                        .FirstOrDefault(m => m.Blueprint.Name == "Barrack") != null ?
                        context.Buildings.Where(m => m.Country.Id == activeCountryId)
                        .First(m => m.Blueprint.Name == "Barrack").NumberOfBuildings*200 >= (armySize + amount) : false;

                    if (country.Gold >= cost * amount && enoughPlace)
                    {
                        Army standing = country.StandingForce.FirstOrDefault(m => m.Type == context.UnitTypes.First(k => k.Id == id));
                        UnitType ut = context.UnitTypes.First(m => m.Id == id);
                        if (standing == null)
                        {
                            standing = new Army()
                            {
                                Type = ut,
                                Size = amount
                            };
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

        public static AssaultViewModel MakeAssaultViewModel()
        {
            using (var context = new ApplicationDbContext())
            {
                int activeCountryId = context.Users.First(c => c.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;
                AssaultViewModel vmAssault = new AssaultViewModel();

                vmAssault.OwnName = context.Countries.FirstOrDefault(m => m.Id == activeCountryId).Name;

                List<Country> countries = new List<Country>() ;               
                foreach (var item in context.Countries.ToList<Country>())
                {
                        Country c = new Country();
                        c.Id = item.Id;
                        c.Name = item.Name;
                        countries.Add(c);
                }

                vmAssault.Countries = countries;

                Country country = context.Countries.First(m => m.Id == activeCountryId);

                List<Army> armies = new List<Army>();

                armies = country.StandingForce.Select(m => new Army()
                {
                    Size = m.Size,
                    Type = new UnitType()
                    {
                        Name = m.Type.Name,
                        Attack = m.Type.Attack,
                        Defense = m.Type.Defense,
                        Description = m.Type.Description
                    }
                }).ToList();
                /*
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
                }*/

                
               // vmAssault.Armies = country.StandingForce.ToList<Army>();
                vmAssault.Armies = armies;

                List<AssaultsCollectModel> cmAssault = new List<AssaultsCollectModel>();

                foreach (var item in country.Assaults.ToList<Assault>())
                {
                    AssaultsCollectModel assault = new AssaultsCollectModel();

                    Country c = new Country()
                    {
                        Id = item.Target.Id,
                        Name = item.Target.Name
                    };

                    assault.Country = c;

                    List<Force> newForces = new List<Force>();

                    newForces = item.Forces.Select(
                        m => new Force(){
                            Size = m.Size,
                            Type = new UnitType()
                            {
                                Id = m.Type.Id,
                                Name = m.Type.Name,
                            }
                        }
                     ).ToList();

                    /*foreach (var f in item.Forces.ToList<Force>())
                    {
                        Force newForce = new Force()
                        {
                            Size = f.Size
                        };
                        newForce.Size = f.Size;
                        UnitType newType = new UnitType();
                        newType.Id = f.Type.Id;
                        newType.Name = f.Type.Name;
                        newForce.Type = newType;
                        newForces.Add(newForce);
                    }*/

                    assault.Forces = newForces;
                    cmAssault.Add(assault);
                }

                vmAssault.Assaults = cmAssault;
                return vmAssault;
            }
        }

        public static void BuildAssault(string name, int[] forces)
        {
            using (var context = new ApplicationDbContext())
            {
                Country c = context.Countries.FirstOrDefault(m => m.Name == name);
                int activeCountryId = context.Users.First(m => m.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;
                if (c != null)
                {
                    Country origin = context.Countries.FirstOrDefault(m => m.Id == activeCountryId);

                    Force archers = new Force()
                    {
                        Size = forces[0],
                        Type  = context.UnitTypes.First(m => m.Name == "Archer")
                    };
                    if(origin.StandingForce.FirstOrDefault(m => m.Type.Name == "Archer") != null)
                        origin.StandingForce.First(m => m.Type.Name == "Archer").Size = origin.StandingForce.First(m => m.Type.Name == "Archer").Size - forces[0];
                    List<Force> forcesList = new List<Force>();
                    forcesList.Add(archers);

                    Force knights = new Force()
                    {
                        Size = forces[1],
                        Type = context.UnitTypes.First(m => m.Name == "Knight")
                    };

                    if (origin.StandingForce.FirstOrDefault(m => m.Type.Name == "Knight") != null)
                        origin.StandingForce.First(m => m.Type.Name == "Knight").Size = origin.StandingForce.First(m => m.Type.Name == "Knight").Size - forces[1];
                    forcesList.Add(knights);

                    Force Elites = new Force()
                    {
                        Size = forces[2],
                        Type = context.UnitTypes.First(m => m.Name == "Elite")
                    };

                    if (origin.StandingForce.FirstOrDefault(m => m.Type.Name == "Elite") != null)
                     origin.StandingForce.First(m => m.Type.Name == "Elite").Size = origin.StandingForce.First(m => m.Type.Name == "Elite").Size - forces[2];
                    forcesList.Add(Elites);

                    Assault assault = new Assault();
                    assault.Forces = forcesList;
                    assault.Target = c;

                    origin.Assaults.Add(assault);

                    context.Assaults.Add(assault);
                    context.SaveChanges();
                }
                
            }
        }
    }
}