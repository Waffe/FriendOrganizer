namespace FriendOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedLocationToMeeting : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Meeting", "Location", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Meeting", "Location");
        }
    }
}
