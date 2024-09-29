using OnlineTicketBookingSystem.Models;

namespace OnlineTicketBookingSystem.DAL.Repository.IRepository
{
    public interface IRoleRepository : IRepository<Role>
    {
        void Update(Role role);
    }
}
