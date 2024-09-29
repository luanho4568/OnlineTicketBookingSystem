using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineTicketBookingSystem.Models
{
    public class GroupRole
    {
        [Key]
        public int Id { get; set; }

        public int GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        [ValidateNever]
        public Group Group { get; set; }

        public int RoleId { get; set; }
        [ForeignKey(nameof(RoleId))]
        [ValidateNever]
        public Role Role { get; set; }
    }
}
