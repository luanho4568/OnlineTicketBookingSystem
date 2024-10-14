using Microsoft.EntityFrameworkCore;
using OnlineTicketBookingSystem.Models;

namespace OnlineTicketBookingSystem.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<AdministrativeRegion> AdministrativeRegions { get; set; }
        public DbSet<AdministrativeUnit> AdministrativeUnits { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<GroupRole> GroupRoles { get; set; }
        public DbSet<Buses> Buses { get; set; }
        public DbSet<Seats> Seats { get; set; }
        public DbSet<Trips> Trips { get; set; }
        public DbSet<Tickets> Tickets { get; set; }
        public DbSet<TripAssignments> TripAssignments { get; set; }
    }
}
