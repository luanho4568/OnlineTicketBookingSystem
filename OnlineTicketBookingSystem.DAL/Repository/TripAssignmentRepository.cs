using OnlineTicketBookingSystem.DAL.Data;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Models;

namespace OnlineTicketBookingSystem.DAL.Repository
{
    public class TripAssignmentRepository : Repository<TripAssignments>, ITripAssignmentRepository
    {
        private readonly ApplicationDbContext _db;
        public TripAssignmentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(TripAssignments tripAssignments)
        {
            _db.TripAssignments.Update(tripAssignments);
        }
    }
}
