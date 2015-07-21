namespace EnCoOrszag.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using EnCoOrszag.Models.DataAccess.Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<EnCoOrszag.Models.DataAccess.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(EnCoOrszag.Models.DataAccess.ApplicationDbContext context)
        {
            context.Blueprints.AddOrUpdate(
                m => m.Name,
                new Blueprints
                {
                    BuildTime = 5,
                    Cost = 0,
                    Description = "Increases your population by 50 and produces 200 potato a turn.",
                    Repeatable = true,
                    Score = 50,
                    Name = "Cottage"
                },
                new Blueprints
                {
                    BuildTime = 5,
                    Cost = 0,
                    Description = "Houses 200 solider.",
                    Repeatable = true,
                    Score = 50,
                    Name = "Barrack"
                }
                );

            context.UnitTypes.AddOrUpdate(
                m => m.Name,
                new UnitType
                {
                    Name = "Archer",
                    Description = "Powerfull defensive unit",
                    Attack = 2,
                    Defense = 6,
                    Cost = 50,
                    Upkeep = 1,
                    Payment = 1,
                    Score = 5
                },
                new UnitType
                {
                    Name = "Knight",
                    Description = "Powerfull assault unit.",
                    Attack = 6,
                    Defense = 2,
                    Cost = 50,
                    Upkeep = 1,
                    Payment = 1,
                    Score = 5
                },
                new UnitType
                {
                    Name = "Elite",
                    Description = "Versatile but expensive special unit.",
                    Attack = 5,
                    Defense = 5,
                    Cost = 100,
                    Upkeep = 2,
                    Payment = 3,
                    Score = 10
                }
            );

            context.Game.AddOrUpdate(
                m => m.Turn,
                new Game
                {
                    Turn = 0
                }
             );

            context.Technologies.AddOrUpdate(
                m => m.Name,
                new Technology
                {
                    Name = "Tractor",
                    Description = "Increases your potato production by 10%",
                    Cost = 0,
                    Repeatable = false,
                    ResearchTime = 15,
                    Score = 100
                },
                new Technology
                {
                    Name = "Harvester",
                    Description = "Increases your potato production by 15%",
                    Cost = 0,
                    Repeatable = false,
                    ResearchTime = 15,
                    Score = 100
                },
                new Technology
                {
                    Name = "City walls",
                    Description = "Increases your defensive powers by 20%",
                    Cost = 0,
                    Repeatable = false,
                    ResearchTime = 15,
                    Score = 100
                },
                new Technology
                {
                    Name = "Operation Rebirth",
                    Description = "Increases your assault powers by 20%",
                    Cost = 0,
                    Repeatable = false,
                    ResearchTime = 15,
                    Score = 100
                },
                new Technology
                {
                    Name = "Tactics",
                    Description = "Increases both your assault and defensive powers by 10%",
                    Cost = 0,
                    Repeatable = false,
                    ResearchTime = 15,
                    Score = 100
                },
                new Technology
                {
                    Name = "Alchemy",
                    Description = "Increases your tax rate by 30%",
                    Cost = 0,
                    Repeatable = false,
                    ResearchTime = 15,
                    Score = 100
                }
                );

            
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
