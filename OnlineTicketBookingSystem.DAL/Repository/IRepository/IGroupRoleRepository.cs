using OnlineTicketBookingSystem.Models;

namespace OnlineTicketBookingSystem.DAL.Repository.IRepository
{
    public interface IGroupRoleRepository : IRepository<GroupRole>
    {
        void Update(GroupRole groupRole);
    }
}
