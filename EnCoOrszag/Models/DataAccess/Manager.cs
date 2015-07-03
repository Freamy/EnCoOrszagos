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
        readonly int MAX_PARALLEL_CONSTRUCTIONS = 1;
        readonly int MAX_PARALLEL_RESEARCHES = 1;

        //Minta beiiras - igazabol rosz, direct ID, etc
        public int getGoldAmount(CountryViewModel mvC)
        {
            int g = 0;
            using (var db = new ApplicationDbContext())
            {
                Country c = db.Countries.Find(1);
                c.Gold += mvC.Gold;
                db.SaveChanges();
            }
            return g;
        }

        public bool isLogedIn()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return true;
            }
            return false;
        }

        public BuildingViewModel makeBuildingViewModel()
        {
            using (var context = new ApplicationDbContext())
            {
                List<Blueprints> bls = context.Blueprints.ToList<Blueprints>();

                List<Building> buildingList = context.Buildings.ToList<Building>();

                List<BlueprintsViewModel> vmBls = new List<BlueprintsViewModel>();

                foreach (var item in bls)
                {
                    BlueprintsViewModel vmTemp = new BlueprintsViewModel();
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

                BuildingViewModel vmBuild = new BuildingViewModel();
                vmBuild.Blueprints = vmBls;

                return vmBuild;
            }
        }

        public ResearchViewModel makeResearchViewModel()
        {
            using (var context = new ApplicationDbContext())
            {
                ResearchViewModel vmResearch = new ResearchViewModel();

                List<TechnologyViewModel> vmTech = new List<TechnologyViewModel>();

                List<Technology> techList = context.Technologies.ToList<Technology>();

                foreach (var tech in techList)
                {
                    TechnologyViewModel temp = new TechnologyViewModel();
                    temp.Name = tech.Name;
                    temp.Description = tech.Description;
                    //TODO: researched figure out!
                    temp.Researched = false;
                    vmTech.Add(temp);
                }

                vmResearch.Technologies = vmTech;
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

        public string getBlueprintName(string submit)
        {
            using (var context = new ApplicationDbContext())
            {
                //return context.Blueprints.Single(m => m.Name == "Build " + submit).Name;
                return context.Blueprints.First(m => m.Name == submit).Name;
            }
        }

        public bool startConstruction(string buildingName)
        {
            using (var context = new ApplicationDbContext())
            {
                // string name = System.Web.HttpContext.Current.User.Identity.Name.ToString(); // How to get current user name
                int activeCountryId = context.Users.First(c => c.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;

                bool canConstruct = context.Constructions.Count(
                    m => m.Country.Id == activeCountryId) < MAX_PARALLEL_CONSTRUCTIONS;

                if (canConstruct)
                {
                    Construction buildingStarted = new Construction();
                    buildingStarted.Blueprint = context.Blueprints.First(m => m.Name == buildingName);

                    buildingStarted.Country = context.Countries.First(
                        m => m.Id == activeCountryId);

                    buildingStarted.FinishTurn = 5; //TODO: current round + 5
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

                    researchStarted.FinishTurn = 15; //TODO: current round + 15
                    context.Researching.Add(researchStarted);
                    context.SaveChanges();
                    return true;
                }

                return false;
            }
        }


        public void endTurn()
        {
            finishConstruction();
            finishResearch();
        }

        public void finishConstruction()
        {
                using (var context = new ApplicationDbContext())
                {
                    int activeCountryId = context.Users.First(c => c.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;

                    //TODO: this needs a where time == 0... korvege thingy
                    Construction construction = context.Constructions.FirstOrDefault(
                        m => m.Country.Id == activeCountryId
                        );

                    if (construction != null)
                    {
                        bool wasBuilding = true;
                        Building building = context.Buildings.FirstOrDefault(
                                m => m.Blueprint.Id == construction.Blueprint.Id
                            );

                        if (building == null)
                        {
                            wasBuilding = false;
                        }

                        if (!wasBuilding)
                            building = new Building();

                        building.Blueprint = construction.Blueprint;
                        building.Country = context.Countries.First(m => m.Id == activeCountryId);
                        building.NumberOfBuildings = building.NumberOfBuildings + 1;

                        context.Constructions.Remove(construction);

                        if (!wasBuilding)
                            context.Buildings.Add(building);

                        context.SaveChanges();
                    }
                }
        }

        public void finishResearch()
        {
            using (var context = new ApplicationDbContext())
            {
                int activeCountryId = context.Users.First(c => c.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;

                Researching researching = context.Researching.FirstOrDefault(
                    m => m.Country.Id == activeCountryId
                );

                if (researching != null)
                {
                    Research finished = new Research();
                    finished.Country = context.Countries.First(m => m.Id == activeCountryId);
                    finished.Finished = 1;

                    context.Researching.Remove(researching);

                    context.Researches.Add(finished);
                    context.SaveChanges();
                }
            }
        }
    }
}