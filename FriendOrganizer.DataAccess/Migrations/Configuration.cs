using FriendOrganizer.Model;

namespace FriendOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FriendOrganizer.DataAccess.FriendOrganizerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FriendOrganizer.DataAccess.FriendOrganizerDbContext context)
        {
            context.Friends.AddOrUpdate(f=>f.FirstName,
                new Friend { FirstName = "Daniel", LastName = "Andersson" },
                new Friend { FirstName = "Fredik", LastName = "Fandersson" },
                new Friend { FirstName = "Huber", LastName = "Carlsson" },
                new Friend { FirstName = "Juber", LastName = "Andersson" });
        }
    }
}
