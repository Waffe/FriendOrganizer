﻿using System;
using System.Data.Entity;
using System.Threading.Tasks;
using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Repositories
{
    class FriendRepository : GenericRepository<Friend, FriendOrganizerDbContext>, IFriendRepository
    {

        private FriendOrganizerDbContext _context;

        public FriendRepository(FriendOrganizerDbContext context)
            :base(context)
        {
            _context = context;
        }
        public override async Task<Friend> GetByIdAsync(int friendId)
        {
            return await _context.Friends.Include(f => f.PhoneNumbers).SingleAsync(f => f.Id == friendId);
        }

        public void RemovePhoneNumber(FriendPhoneNumber model)
        {
            Context.FriendPhoneNumbers.Remove(model);
        }

    }
}
