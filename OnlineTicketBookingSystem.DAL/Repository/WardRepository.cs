using OnlineTicketBookingSystem.DAL.Data;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Models;

namespace OnlineTicketBookingSystem.DAL.Repository
{
    public class WardRepository : Repository<Ward>, IWardRepository
    {
        private readonly ApplicationDbContext _db;
        public WardRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
