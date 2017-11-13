using System.Collections.Generic;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Repositories;

namespace FriendOrganizer.UI.Data
{
    public interface IFriendRepository:IGenerticRepository<Friend>
    {
        
        void RemovePhoneNumber(FriendPhoneNumber model);
    }
}