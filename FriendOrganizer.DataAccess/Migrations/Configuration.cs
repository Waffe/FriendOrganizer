using System.Collections.Generic;

namespace FriendOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using FriendOrganizer.Model;

    internal sealed class Configuration : DbMigrationsConfiguration<FriendOrganizerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FriendOrganizerDbContext context)
        {
            context.Friends.AddOrUpdate(f=>f.FirstName,
                new Friend { FirstName = "Daniel", LastName = "Andersson" },
                new Friend { FirstName = "Fredik", LastName = "Fandersson" },
                new Friend { FirstName = "Huber", LastName = "Carlsson" },
                new Friend { FirstName = "Juber", LastName = "Andersson" });
            context.ProgrammingLanguages.AddOrUpdate(
                pl => pl.Name,
                new ProgrammingLanguage { Name = "C#" },
                new ProgrammingLanguage { Name = "TypeScript" },
                new ProgrammingLanguage { Name = "F#" },
                new ProgrammingLanguage { Name = "Swift" },
                new ProgrammingLanguage { Name = "Java" });


            context.SaveChanges();

            context.FriendPhoneNumbers.AddOrUpdate(pn => pn.Number,
                new FriendPhoneNumber { Number = "+49 12345678", FriendId = context.Friends.First().Id });


            context.Meetings.AddOrUpdate(m=>m.Title, new Meeting()
            {
                Title = "Watching Soccer",
                DateFrom = new DateTime(2018,5,26),
                DateTo = new DateTime(2018,5,26),
                Friends = new List<Friend>
                {
                    context.Friends.Single(f => f.FirstName == "Juber" && f.LastName == "Andersson"),
                    context.Friends.Single(f=>f.FirstName == "Huber" && f.LastName == "Carlsson")
                }
            });
        }
    }
}
