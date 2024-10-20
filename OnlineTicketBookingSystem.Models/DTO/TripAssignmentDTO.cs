using System.ComponentModel.DataAnnotations;

namespace OnlineTicketBookingSystem.Models.DTO
{
    public class TripAssignmentDTO
    {
        [Required]
        public Guid DriverId { get; set; }

        [Required]
        public Guid TripId { get; set; }
    }
}
