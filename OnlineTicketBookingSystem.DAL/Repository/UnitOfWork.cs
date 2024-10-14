using OnlineTicketBookingSystem.DAL.Data;
using OnlineTicketBookingSystem.DAL.Repository.IRepository;

namespace OnlineTicketBookingSystem.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        public IProvinceRepository Province { get; private set; }

        public IDistrictRepository District { get; private set; }

        public IWardRepository Ward { get; private set; }


        public IUserRepository User { get; private set; }

        public IGroupRepository Group { get; private set; }

        public IRoleRepository Role { get; private set; }

        public IGroupRoleRepository GroupRole { get; private set; }

        public IBusRepository Buses { get; private set; }

        public ISeatRepository Seats { get; private set; }

        public ITripRepository Trips { get; private set; }

        public ITripAssignmentRepository TripsAssignments { get; private set; }

        public ITicketRepository Tickets { get; private set; }

        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Province = new ProvinceRepository(_db);
            District = new DistrictRepository(_db);
            Ward = new WardRepository(_db);
            User = new UserRepository(_db);
            Group = new GroupRepository(_db);
            Role = new RoleRepository(_db);
            GroupRole = new GroupRoleRepository(_db);
            Buses = new BusRepository(_db);
            Seats = new SeatRepository(_db);
            Trips = new TripRepository(_db);
            TripsAssignments = new TripAssignmentRepository(_db);
            Tickets = new TicketRepository(_db);
        }


        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
