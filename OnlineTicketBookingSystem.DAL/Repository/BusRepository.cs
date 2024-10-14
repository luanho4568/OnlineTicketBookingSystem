using OnlineTicketBookingSystem.DAL.Data;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Models;

namespace OnlineTicketBookingSystem.DAL.Repository
{
    public class BusRepository : Repository<Buses>, IBusRepository
    {
        private readonly ApplicationDbContext _db;
        public BusRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Buses buses)
        {
            _db.Buses.Update(buses);
        }
    }
}
