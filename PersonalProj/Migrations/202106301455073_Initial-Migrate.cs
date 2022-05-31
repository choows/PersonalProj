namespace PersonalProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigrate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Expenses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UpdateDateTime = c.DateTime(nullable: false),
                        ExpensesDateTime = c.DateTime(nullable: false),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Leow = c.Decimal(precision: 18, scale: 2),
                        Choo = c.Decimal(precision: 18, scale: 2),
                        LeowPaid = c.Decimal(precision: 18, scale: 2),
                        ChooPaid = c.Decimal(precision: 18, scale: 2),
                        ImagePath = c.String(),
                        Place_ID = c.Guid(),
                        UploadUser_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Place", t => t.Place_ID)
                .ForeignKey("dbo.User", t => t.UploadUser_ID)
                .Index(t => t.Place_ID)
                .Index(t => t.UploadUser_ID);
            
            CreateTable(
                "dbo.Place",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Password = c.String(),
                        LastLogin = c.DateTime(nullable: false),
                        UserIpAddress = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Media",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Type = c.String(),
                        LocalPath = c.String(),
                        FileName = c.String(),
                        UploadDate = c.DateTime(nullable: false),
                        UploadUserID = c.Int(nullable: false),
                        User_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.User_ID)
                .Index(t => t.User_ID);
            
            CreateTable(
                "dbo.History",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Details = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Memo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UpdateDateTime = c.DateTime(nullable: false),
                        Memo_Details = c.String(),
                        Status = c.String(),
                        TextColor = c.String(),
                        BackgroundColro = c.String(),
                        Priority = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Expenses", "UploadUser_ID", "dbo.User");
            DropForeignKey("dbo.Media", "User_ID", "dbo.User");
            DropForeignKey("dbo.Expenses", "Place_ID", "dbo.Place");
            DropIndex("dbo.Media", new[] { "User_ID" });
            DropIndex("dbo.Expenses", new[] { "UploadUser_ID" });
            DropIndex("dbo.Expenses", new[] { "Place_ID" });
            DropTable("dbo.Memo");
            DropTable("dbo.History");
            DropTable("dbo.Media");
            DropTable("dbo.User");
            DropTable("dbo.Place");
            DropTable("dbo.Expenses");
        }
    }
}
