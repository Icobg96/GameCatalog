namespace GameCatalog.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ratings", "GreatYear", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ratings", "GreatYear");
        }
    }
}
