namespace Chat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(maxLength: 1000),
                        AuthorId = c.Int(nullable: false),
                        RecipientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AuthorId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.RecipientId, cascadeDelete: false)
                .Index(t => t.AuthorId)
                .Index(t => t.RecipientId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(nullable: false),
                        Password = c.Guid(nullable: false),
                        DateOfBirth = c.DateTime(),
                        DateRegister = c.DateTime(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        About = c.String(),
                        Hide = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "RecipientId", "dbo.Users");
            DropForeignKey("dbo.Messages", "AuthorId", "dbo.Users");
            DropIndex("dbo.Messages", new[] { "RecipientId" });
            DropIndex("dbo.Messages", new[] { "AuthorId" });
            DropTable("dbo.Users");
            DropTable("dbo.Messages");
        }
    }
}
