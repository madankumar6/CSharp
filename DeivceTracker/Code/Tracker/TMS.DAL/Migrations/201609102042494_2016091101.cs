namespace TMS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2016091101 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeviceModels", "EntryDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.DeviceModels", "Entry_Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DeviceModels", "Entry_Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.DeviceModels", "EntryDate");
        }
    }
}
