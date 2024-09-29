using System.ComponentModel.DataAnnotations;

namespace OnlineTicketBookingSystem.Models
{
    public class AdministrativeRegion
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? NameEn { get; set; }

        public string? CodeName { get; set; }
        public string? CodeNameEn { get; set; }
    }
}
