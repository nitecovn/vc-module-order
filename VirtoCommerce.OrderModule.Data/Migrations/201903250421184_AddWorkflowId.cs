namespace VirtoCommerce.OrderModule.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWorkflowId : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerOrderWorkflow",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        WorkflowId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerOrder", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerOrderWorkflow", "Id", "dbo.CustomerOrder");
            DropIndex("dbo.CustomerOrderWorkflow", new[] { "Id" });
            DropTable("dbo.CustomerOrderWorkflow");
        }
    }
}
