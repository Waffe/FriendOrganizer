﻿using System.Collections.Generic;
using System.Threading.Tasks;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data
{
    public interface IFriendRepository
    {
        Task<Friend> GetByIdAsync(int friendId);
        Task SaveAsync();
        bool HasChanges();
    }
}