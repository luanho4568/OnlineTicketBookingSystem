using System.ComponentModel.DataAnnotations;

namespace OnlineTicketBookingSystem.Models
{
    public class Buses
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Name { get; set; }
        public string? LicensePlate { get; set; }
        public string? BusType { get; set; }
        public int? TotalSeats { get; set; }
        public bool Status { get; set; }
        public string? Image { get; set; }
        public string? CreatedAt { get; set; } = DateTime.Now.ToString();
        public string? UpdatedAt { get; set; }
    }
}
