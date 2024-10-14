using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineTicketBookingSystem.Models
{
    public class Tickets
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
        public Guid? TripId { get; set; }
        [ForeignKey(nameof(TripId))]
        public Trips? Trips { get; set; }
        public Guid? SeatId { get; set; }
        [ForeignKey(nameof(SeatId))]
        public Seats? Seats { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public string? Status { get; set; }
        public string? CreatedAt { get; set; } = DateTime.Now.ToString();
    }
}
