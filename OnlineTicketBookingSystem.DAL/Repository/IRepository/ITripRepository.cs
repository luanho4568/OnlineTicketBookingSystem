using OnlineTicketBookingSystem.Models;

namespace OnlineTicketBookingSystem.DAL.Repository.IRepository
{
    public interface ITripRepository : IRepository<Trips>
    {
        void Update(Trips trips);
    }
}
