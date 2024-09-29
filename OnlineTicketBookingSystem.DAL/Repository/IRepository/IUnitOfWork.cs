
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
        Task SaveAsync();
    }
}
