using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineTicketBookingSystem.Models.ViewModel
{
    public class UserVM
    {
        public User User { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Group { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Province { get; set; }

    }
}
