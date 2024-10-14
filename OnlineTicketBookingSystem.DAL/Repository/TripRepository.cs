using OnlineTicketBookingSystem.DAL.Data;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Models;

namespace OnlineTicketBookingSystem.DAL.Repository
{
    public class TripRepository : Repository<Trips>, ITripRepository
    {
        private readonly ApplicationDbContext _db;
        public TripRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Trips trips)
        {
            _db.Trips.Update(trips);
        }
    }
}
