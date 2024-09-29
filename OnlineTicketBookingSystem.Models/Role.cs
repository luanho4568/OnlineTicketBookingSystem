using System.ComponentModel.DataAnnotations;

namespace OnlineTicketBookingSystem.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string RoleUrl { get; set; }
        public string Description { get; set; }
    }
}
