using OnlineTicketBookingSystem.Models;

namespace OnlineTicketBookingSystem.DAL.Repository.IRepository
{
    public interface ITripAssignmentRepository : IRepository<TripAssignments>
    {
        void Update(TripAssignments tripAssignments);
    }
}
