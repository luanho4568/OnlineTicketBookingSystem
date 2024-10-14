using OnlineTicketBookingSystem.Models;

namespace OnlineTicketBookingSystem.DAL.Repository.IRepository
{
    public interface ISeatRepository : IRepository<Seats>
    {
        void Update(Seats seats);
    }
}
