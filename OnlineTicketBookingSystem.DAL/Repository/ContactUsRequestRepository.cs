using OnlineTicketBookingSystem.DAL.Data;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;
using OnlineTicketBookingSystem.Models;

namespace OnlineTicketBookingSystem.DAL.Repository
{
    public class ContactUsRequestRepository : Repository<ContactUsRequest>, IContactUsRequestRepository
    {
        private readonly ApplicationDbContext _db;
        public ContactUsRequestRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
