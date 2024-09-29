using OnlineTicketBookingSystem.DAL.Data;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Models;

namespace OnlineTicketBookingSystem.DAL.Repository
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private readonly ApplicationDbContext _db;
        public RoleRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Role role)
        {
            throw new NotImplementedException();
        }
    }
}
