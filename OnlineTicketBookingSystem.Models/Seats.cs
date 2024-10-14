using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineTicketBookingSystem.Models
{
    public class Seats
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? BusId { get; set; }
        [ForeignKey(nameof(BusId))]
        [ValidateNever]
        public Buses? Buses { get; set; }
        public string? SeatNumber { get; set; }
        public string? Status { get; set; }
    }
}
