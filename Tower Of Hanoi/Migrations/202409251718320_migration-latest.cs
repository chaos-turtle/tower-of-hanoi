namespace Tower_Of_Hanoi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrationlatest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Scores", "Disks", c => c.Int(nullable: false));
            AddColumn("dbo.Scores", "IsPerfect", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Scores", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Scores", "UserId");
            AddForeignKey("dbo.Scores", "UserId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Scores", "DiskCount");
            DropColumn("dbo.Scores", "IsPerfectScore");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Scores", "IsPerfectScore", c => c.Boolean(nullable: false));
            AddColumn("dbo.Scores", "DiskCount", c => c.Int(nullable: false));
            DropForeignKey("dbo.Scores", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Scores", new[] { "UserId" });
            AlterColumn("dbo.Scores", "UserId", c => c.String());
            DropColumn("dbo.Scores", "IsPerfect");
            DropColumn("dbo.Scores", "Disks");
        }
    }
}
