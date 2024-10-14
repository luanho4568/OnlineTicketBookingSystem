using OnlineTicketBookingSystem.Models;

namespace OnlineTicketBookingSystem.DAL.Repository.IRepository
{
    public interface ITicketRepository : IRepository<Tickets>
    {
        void Update(Tickets tickes);
    }
}
