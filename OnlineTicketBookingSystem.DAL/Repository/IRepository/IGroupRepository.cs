using OnlineTicketBookingSystem.Models;

namespace OnlineTicketBookingSystem.DAL.Repository.IRepository
{
    public interface IGroupRepository : IRepository<Group>
    {
        void Update(Group group);
    }
}
