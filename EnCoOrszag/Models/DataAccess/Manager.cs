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

        public BuildingViewModel makeBuildingViewModel()
        {
            using (var context = new ApplicationDbContext())
            {
                List<Blueprints> bls = context.Blueprints.ToList<Blueprints>();
                //List<Building> buildingList = context.Buildings.ToList<Building>();

                List<BlueprintsViewModel> vmBls = new List<BlueprintsViewModel>();

                foreach (var item in bls)
                {
                    BlueprintsViewModel vmTemp = new BlueprintsViewModel();
                    vmTemp.Name = item.Name;
                    vmTemp.BuildTime = item.BuildTime;
                    vmTemp.Cost = item.Cost;
                    vmTemp.Description = item.Description;
                    vmTemp.Repeatable = item.Repeatable;
                    /*foreach (var building in buildingList)
                    {
                        if(building.Blueprint == item){
                            vmTemp.NoOfFinishedBlueprints = building.NumberOfBuildings;
                            break;
                        }
                    }*/
                    
                    vmBls.Add(vmTemp);
                }


                

                BuildingViewModel vmBuild = new BuildingViewModel();
                vmBuild.Blueprints = vmBls;

                return vmBuild;
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
                return context.Blueprints.First(m => "Build " + m.Name == submit).Name;
            }
        }

        public bool startConstruction(string buildingName)
        {
            using (var context = new ApplicationDbContext())
            {
                // string name = System.Web.HttpContext.Current.User.Identity.Name.ToString(); // How to get current user name
                int activeCountryId = context.Users.First(c => c.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;

                bool canConstruct = context.Constructions.Count(
                    m => m.Country.Id == activeCountryId) == 0;

                if (canConstruct)
                {
                    Construction buildingStarted = new Construction();
                    buildingStarted.Blueprint = context.Blueprints.First(m => m.Name == buildingName);
                    buildingStarted.Country = context.Countries.First(
                        m => m.Id == activeCountryId);
                    buildingStarted.FinishTurn = 5;
                    context.Constructions.Add(buildingStarted);
                    context.SaveChanges();
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        /*  TODO: This function will need to find the finishing construction, right now it just returns the only one the player have.
            It can actually stay this way since right now you can only build one thing at a time. */
        public Construction finishingConstruction()
        {
            using (var context = new ApplicationDbContext())
            {
                int activeCountryId = context.Users.First(c => c.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;
                return context.Constructions.FirstOrDefault(
                        m => m.Country.Id == activeCountryId
                        );
            }
        }

        public void finishConstruction()
        {
           // if(construction != null)
                using (var context = new ApplicationDbContext())
                {
                    int activeCountryId = context.Users.First(c => c.UserName == System.Web.HttpContext.Current.User.Identity.Name).Country.Id;
                    Construction construction = context.Constructions.FirstOrDefault(
                        m => m.Country.Id == activeCountryId
                        );


                    bool wasBuilding = true;
                    Building building = context.Buildings.FirstOrDefault( 
                            m => m.Blueprint.Id == construction.Blueprint.Id
                        );

                    if (building == null)
                    {
                        wasBuilding = false;
                    }

                    if(!wasBuilding)
                        building = new Building();

                    building.Blueprint = construction.Blueprint;
                    building.Country = context.Countries.First(m => m.Id == activeCountryId);
                    building.NumberOfBuildings = building.NumberOfBuildings + 1;
                                
                    context.Constructions.Remove(construction);

                    if(!wasBuilding)
                        context.Buildings.Add(building);

                    context.SaveChanges();
                }
        }
    }
}