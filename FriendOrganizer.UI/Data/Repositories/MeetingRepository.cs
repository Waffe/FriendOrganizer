using System.Threading.Tasks;
using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System.Linq;
using System.Data.Entity;

namespace FriendOrganizer.UI.Data.Repositories
{
    public class MeetingRepository : GenericRepository<Meeting, FriendOrganizerDbContext>, IMeetingRepository
    {
        public MeetingRepository(FriendOrganizerDbContext context) : base(context)
        {
            
        }

        public override async Task<Meeting> GetByIdAsync(int id)
        {
            return await Context.Meetings.Include(x => x.Friends).SingleAsync(m => m.Id == id);
        }
    }
}
