namespace PersonalProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Expenses", "description_head", c => c.String());
            AddColumn("dbo.Expenses", "description_tail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Expenses", "description_tail");
            DropColumn("dbo.Expenses", "description_head");
        }
    }
}
