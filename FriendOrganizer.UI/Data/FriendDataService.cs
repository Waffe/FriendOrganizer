using FriendOrganizer.Model;
using System.Collections.Generic;
using System.Linq;
using FriendOrganizer.DataAccess;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data
{
    class FriendDataService : IFriendDataService
    {
        private Func<FriendOrganizerDbContext> _contextCreator;

        public FriendDataService(Func<FriendOrganizerDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }
        public async Task<Friend> GetByIdAsync(int friendId)
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Friends.AsNoTracking().SingleAsync(f=>f.Id==friendId);
            }
        }
    }
}
