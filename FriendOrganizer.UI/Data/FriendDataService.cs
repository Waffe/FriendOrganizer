using FriendOrganizer.Model;
using System.Collections.Generic;

namespace FriendOrganizer.UI.Data
{
    class FriendDataService : IFriendDataService
    {      
        public IEnumerable<Friend> GetAll()
        {
            // TODO: Load data from a real database
            yield return new Friend { FirstName = "Daniel", LastName = "Andersson" };
            yield return new Friend { FirstName = "Fredik", LastName = "Fandersson" };
            yield return new Friend { FirstName = "Huber", LastName = "Carlsson" };
            yield return new Friend { FirstName = "Juber", LastName = "Andersson" };
        }
    }
}
