using OnlineTicketBookingSystem.DAL.Data;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Models;

namespace OnlineTicketBookingSystem.DAL.Repository
{
    public class GroupRoleRepository : Repository<GroupRole>, IGroupRoleRepository
    {
        private readonly ApplicationDbContext _db;
        public GroupRoleRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(GroupRole groupRole)
        {
            throw new NotImplementedException();
        }
    }
}
