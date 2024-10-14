using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineTicketBookingSystem.Models.ViewModel
{
    public class TripVM
    {
        public Trips Trip { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> Province { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> Buses { get; set; }
    }
}
