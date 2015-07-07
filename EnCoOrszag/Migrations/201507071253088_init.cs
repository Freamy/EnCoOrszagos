namespace EnCoOrszag.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Armies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Country_Id = c.Int(),
                        Origin_Id = c.Int(),
                        TargetCountry_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.Country_Id)
                .ForeignKey("dbo.Countries", t => t.Origin_Id)
                .ForeignKey("dbo.Countries", t => t.TargetCountry_Id)
                .Index(t => t.Country_Id)
                .Index(t => t.Origin_Id)
                .Index(t => t.TargetCountry_Id);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Size = c.Int(nullable: false),
                        Army_Id = c.Int(),
                        Country_Id = c.Int(),
                        UnitType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Armies", t => t.Army_Id)
                .ForeignKey("dbo.Countries", t => t.Country_Id)
                .ForeignKey("dbo.UnitTypes", t => t.UnitType_Id)
                .Index(t => t.Army_Id)
                .Index(t => t.Country_Id)
                .Index(t => t.UnitType_Id);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Gold = c.Int(nullable: false),
                        Potato = c.Int(nullable: false),
                        Population = c.Int(nullable: false),
                        Score = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Buildings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumberOfBuildings = c.Int(nullable: false),
                        Blueprint_Id = c.Int(),
                        Country_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Blueprints", t => t.Blueprint_Id)
                .ForeignKey("dbo.Countries", t => t.Country_Id)
                .Index(t => t.Blueprint_Id)
                .Index(t => t.Country_Id);
            
            CreateTable(
                "dbo.Blueprints",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Repeatable = c.Boolean(nullable: false),
                        BuildTime = c.Int(nullable: false),
                        Cost = c.Int(nullable: false),
                        Score = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Constructions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FinishTurn = c.Int(nullable: false),
                        Blueprint_Id = c.Int(),
                        Country_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Blueprints", t => t.Blueprint_Id)
                .ForeignKey("dbo.Countries", t => t.Country_Id)
                .Index(t => t.Blueprint_Id)
                .Index(t => t.Country_Id);
            
            CreateTable(
                "dbo.Researches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Finished = c.Boolean(nullable: false),
                        Country_Id = c.Int(),
                        Technology_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.Country_Id)
                .ForeignKey("dbo.Technologies", t => t.Technology_Id)
                .Index(t => t.Country_Id)
                .Index(t => t.Technology_Id);
            
            CreateTable(
                "dbo.Technologies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Cost = c.Int(nullable: false),
                        ResearchTime = c.Int(nullable: false),
                        Repeatable = c.Boolean(nullable: false),
                        Score = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Researchings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FinishTurn = c.Int(nullable: false),
                        Country_Id = c.Int(),
                        Technology_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.Country_Id)
                .ForeignKey("dbo.Technologies", t => t.Technology_Id)
                .Index(t => t.Country_Id)
                .Index(t => t.Technology_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Country_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.Country_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Country_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.UnitTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Attack = c.Int(nullable: false),
                        Defense = c.Int(nullable: false),
                        Cost = c.Int(nullable: false),
                        Upkeep = c.Int(nullable: false),
                        Payment = c.Int(nullable: false),
                        Score = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Turn = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Armies", "TargetCountry_Id", "dbo.Countries");
            DropForeignKey("dbo.Armies", "Origin_Id", "dbo.Countries");
            DropForeignKey("dbo.Groups", "UnitType_Id", "dbo.UnitTypes");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Country_Id", "dbo.Countries");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Groups", "Country_Id", "dbo.Countries");
            DropForeignKey("dbo.Researchings", "Technology_Id", "dbo.Technologies");
            DropForeignKey("dbo.Researchings", "Country_Id", "dbo.Countries");
            DropForeignKey("dbo.Researches", "Technology_Id", "dbo.Technologies");
            DropForeignKey("dbo.Researches", "Country_Id", "dbo.Countries");
            DropForeignKey("dbo.Constructions", "Country_Id", "dbo.Countries");
            DropForeignKey("dbo.Constructions", "Blueprint_Id", "dbo.Blueprints");
            DropForeignKey("dbo.Buildings", "Country_Id", "dbo.Countries");
            DropForeignKey("dbo.Buildings", "Blueprint_Id", "dbo.Blueprints");
            DropForeignKey("dbo.Armies", "Country_Id", "dbo.Countries");
            DropForeignKey("dbo.Groups", "Army_Id", "dbo.Armies");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Country_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Researchings", new[] { "Technology_Id" });
            DropIndex("dbo.Researchings", new[] { "Country_Id" });
            DropIndex("dbo.Researches", new[] { "Technology_Id" });
            DropIndex("dbo.Researches", new[] { "Country_Id" });
            DropIndex("dbo.Constructions", new[] { "Country_Id" });
            DropIndex("dbo.Constructions", new[] { "Blueprint_Id" });
            DropIndex("dbo.Buildings", new[] { "Country_Id" });
            DropIndex("dbo.Buildings", new[] { "Blueprint_Id" });
            DropIndex("dbo.Groups", new[] { "UnitType_Id" });
            DropIndex("dbo.Groups", new[] { "Country_Id" });
            DropIndex("dbo.Groups", new[] { "Army_Id" });
            DropIndex("dbo.Armies", new[] { "TargetCountry_Id" });
            DropIndex("dbo.Armies", new[] { "Origin_Id" });
            DropIndex("dbo.Armies", new[] { "Country_Id" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Games");
            DropTable("dbo.UnitTypes");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Researchings");
            DropTable("dbo.Technologies");
            DropTable("dbo.Researches");
            DropTable("dbo.Constructions");
            DropTable("dbo.Blueprints");
            DropTable("dbo.Buildings");
            DropTable("dbo.Countries");
            DropTable("dbo.Groups");
            DropTable("dbo.Armies");
        }
    }
}
