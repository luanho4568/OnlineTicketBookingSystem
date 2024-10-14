using OnlineTicketBookingSystem.Models;

namespace OnlineTicketBookingSystem.DAL.Repository.IRepository
{
    public interface IBusRepository : IRepository<Buses>
    {
        void Update(Buses buses);
    }
}
