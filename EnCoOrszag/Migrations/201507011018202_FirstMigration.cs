namespace EnCoOrszag.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Armies", name: "Country_Id1", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.Armies", name: "Country_Id", newName: "Country_Id1");
            RenameColumn(table: "dbo.Armies", name: "__mig_tmp__0", newName: "Country_Id");
            RenameIndex(table: "dbo.Armies", name: "IX_Country_Id1", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.Armies", name: "IX_Country_Id", newName: "IX_Country_Id1");
            RenameIndex(table: "dbo.Armies", name: "__mig_tmp__0", newName: "IX_Country_Id");
            AddColumn("dbo.UnitTypes", "Payment", c => c.Int(nullable: false));
            AddColumn("dbo.Blueprints", "Name", c => c.String());
            AddColumn("dbo.Blueprints", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Blueprints", "Description");
            DropColumn("dbo.Blueprints", "Name");
            DropColumn("dbo.UnitTypes", "Payment");
            RenameIndex(table: "dbo.Armies", name: "IX_Country_Id", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.Armies", name: "IX_Country_Id1", newName: "IX_Country_Id");
            RenameIndex(table: "dbo.Armies", name: "__mig_tmp__0", newName: "IX_Country_Id1");
            RenameColumn(table: "dbo.Armies", name: "Country_Id", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.Armies", name: "Country_Id1", newName: "Country_Id");
            RenameColumn(table: "dbo.Armies", name: "__mig_tmp__0", newName: "Country_Id1");
        }
    }
}
