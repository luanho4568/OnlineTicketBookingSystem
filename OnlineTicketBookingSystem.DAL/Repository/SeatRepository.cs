using OnlineTicketBookingSystem.DAL.Data;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Models;

namespace OnlineTicketBookingSystem.DAL.Repository
{
    public class SeatRepository : Repository<Seats>, ISeatRepository
    {
        private readonly ApplicationDbContext _db;
        public SeatRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Seats seats)
        {
            _db.Seats.Update(seats);
        }
    }
}
