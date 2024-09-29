using System.ComponentModel.DataAnnotations;

namespace OnlineTicketBookingSystem.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameVI { get; set; }
        public string Description { get; set; }
    }
}
