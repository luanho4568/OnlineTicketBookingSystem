using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineTicketBookingSystem.Models
{
    public class TripAssignments
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? DriverId { get; set; }
        [ForeignKey(nameof(DriverId))]
        public User? Driver { get; set; }
        public Guid? TripId { get; set; }
        [ForeignKey(nameof(TripId))]
        public Trips? Trips { get; set; }
        public string? Status { get; set; }
        public DateTime? CreateAt { get; set; } = DateTime.Now;
    }
}
