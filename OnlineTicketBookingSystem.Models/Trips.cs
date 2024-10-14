using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineTicketBookingSystem.Models
{
    public class Trips
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? BusId { get; set; }
        [ForeignKey(nameof(BusId))]
        [ValidateNever]
        public Buses? Buses { get; set; }
        [ForeignKey(nameof(StartProvince))]
        public string? StartPoint { get; set; }

        [ForeignKey(nameof(EndProvince))]
        public string? EndPoint { get; set; }

        public Province? StartProvince { get; set; }
        public Province? EndProvince { get; set; }
        public DateTime? DepartureDate { get; set; }
        public string? Distance { get; set; }
        public string? DepartureTime { get; set; }
        public string? EstimatedArrivalTime { get; set; }
        public string? Status { get; set; }
        public string? CreatedAt { get; set; } = DateTime.Now.ToString();
        public string? UpdatedAt { get; set; }

    }
}
