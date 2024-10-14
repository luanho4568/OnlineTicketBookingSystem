namespace OnlineTicketBookingSystem.Models.ViewModel
{
    public class TicketVM
    {
        public Guid? UserId { get; set; }
        public Guid? TripId { get; set; }
        public List<Guid?> SeatIds { get; set; }
        public decimal Price { get; set; }
    }
}
