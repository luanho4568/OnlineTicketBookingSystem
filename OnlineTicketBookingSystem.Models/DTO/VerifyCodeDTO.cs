namespace OnlineTicketBookingSystem.Models.DTO
{
    public class VerifyCodeDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? CodeId { get; set; }
    }
}
