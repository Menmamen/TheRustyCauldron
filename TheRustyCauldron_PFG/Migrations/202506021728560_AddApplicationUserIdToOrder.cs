namespace TheRustyCauldron_PFG.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddApplicationUserIdToOrder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        PotionId = c.Int(nullable: false),
                        CustomerName = c.String(nullable: false, maxLength: 255),
                        CustomerAddress = c.String(nullable: false, maxLength: 500),
                        CustomerEmail = c.String(nullable: false, maxLength: 255),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OrderDate = c.DateTime(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Potions", t => t.PotionId, cascadeDelete: true)
                .Index(t => t.PotionId)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "PotionId", "dbo.Potions");
            DropForeignKey("dbo.Orders", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "ApplicationUserId" });
            DropIndex("dbo.Orders", new[] { "PotionId" });
            DropTable("dbo.Orders");
        }
    }
}
