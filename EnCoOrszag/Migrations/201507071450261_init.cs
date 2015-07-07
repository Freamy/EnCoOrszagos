namespace EnCoOrszag.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
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
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Gold = c.Int(nullable: false),
                        Potato = c.Int(nullable: false),
                        Population = c.Int(nullable: false),
                        Score = c.Int(nullable: false),
                        Assault_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assaults", t => t.Assault_Id)
                .Index(t => t.Assault_Id);
            
            CreateTable(
                "dbo.Assaults",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Target_Id = c.Int(),
                        Country_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.Target_Id)
                .ForeignKey("dbo.Countries", t => t.Country_Id)
                .Index(t => t.Target_Id)
                .Index(t => t.Country_Id);
            
            CreateTable(
                "dbo.Forces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Size = c.Int(nullable: false),
                        Type_Id = c.Int(),
                        Assault_Id = c.Int(),
                        Country_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UnitTypes", t => t.Type_Id)
                .ForeignKey("dbo.Assaults", t => t.Assault_Id)
                .ForeignKey("dbo.Countries", t => t.Country_Id)
                .Index(t => t.Type_Id)
                .Index(t => t.Assault_Id)
                .Index(t => t.Country_Id);
            
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
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Country_Id", "dbo.Countries");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Researchings", "Technology_Id", "dbo.Technologies");
            DropForeignKey("dbo.Researchings", "Country_Id", "dbo.Countries");
            DropForeignKey("dbo.Researches", "Technology_Id", "dbo.Technologies");
            DropForeignKey("dbo.Researches", "Country_Id", "dbo.Countries");
            DropForeignKey("dbo.Forces", "Country_Id", "dbo.Countries");
            DropForeignKey("dbo.Constructions", "Country_Id", "dbo.Countries");
            DropForeignKey("dbo.Constructions", "Blueprint_Id", "dbo.Blueprints");
            DropForeignKey("dbo.Buildings", "Country_Id", "dbo.Countries");
            DropForeignKey("dbo.Assaults", "Country_Id", "dbo.Countries");
            DropForeignKey("dbo.Assaults", "Target_Id", "dbo.Countries");
            DropForeignKey("dbo.Countries", "Assault_Id", "dbo.Assaults");
            DropForeignKey("dbo.Forces", "Assault_Id", "dbo.Assaults");
            DropForeignKey("dbo.Forces", "Type_Id", "dbo.UnitTypes");
            DropForeignKey("dbo.Buildings", "Blueprint_Id", "dbo.Blueprints");
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
            DropIndex("dbo.Forces", new[] { "Country_Id" });
            DropIndex("dbo.Forces", new[] { "Assault_Id" });
            DropIndex("dbo.Forces", new[] { "Type_Id" });
            DropIndex("dbo.Assaults", new[] { "Country_Id" });
            DropIndex("dbo.Assaults", new[] { "Target_Id" });
            DropIndex("dbo.Countries", new[] { "Assault_Id" });
            DropIndex("dbo.Buildings", new[] { "Country_Id" });
            DropIndex("dbo.Buildings", new[] { "Blueprint_Id" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Games");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Researchings");
            DropTable("dbo.Technologies");
            DropTable("dbo.Researches");
            DropTable("dbo.Constructions");
            DropTable("dbo.UnitTypes");
            DropTable("dbo.Forces");
            DropTable("dbo.Assaults");
            DropTable("dbo.Countries");
            DropTable("dbo.Buildings");
            DropTable("dbo.Blueprints");
        }
    }
}
