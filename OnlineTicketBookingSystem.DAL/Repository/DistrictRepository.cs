using OnlineTicketBookingSystem.DAL.Data;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Models;

namespace OnlineTicketBookingSystem.DAL.Repository
{
    public class DistrictRepository : Repository<District>, IDistrictRepository
    {
        private readonly ApplicationDbContext _db;
        public DistrictRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
