
namespace OnlineTicketBookingSystem.DAL.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IProvinceRepository Province { get; }
        IDistrictRepository District { get; }
        IWardRepository Ward { get; }
        IUserRepository User { get; }
        IGroupRepository Group { get; }
        IRoleRepository Role { get; }
        IGroupRoleRepository GroupRole { get; }
        IBusRepository Buses { get; }
        ISeatRepository Seats { get; }
        ITripRepository Trips { get; }
        ITripAssignmentRepository TripsAssignments { get; }
        Task SaveAsync();
    }
}
