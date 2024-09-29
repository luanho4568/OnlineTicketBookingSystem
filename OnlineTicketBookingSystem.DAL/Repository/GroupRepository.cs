using OnlineTicketBookingSystem.DAL.Data;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Models;

namespace OnlineTicketBookingSystem.DAL.Repository
{
    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        private readonly ApplicationDbContext _db;
        public GroupRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Group group)
        {
            throw new NotImplementedException();
        }
    }
}
