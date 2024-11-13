using System.ComponentModel.DataAnnotations;

namespace OnlineTicketBookingSystem.Models
{
    public class ContactUsRequest
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Vui lòng điền họ tên")]
        [MinLength(6, ErrorMessage = "Họ và tên ít nhất 6 ký tự.")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Vui lòng điền số điện thoại")]
        [StringLength(11, MinimumLength = 10, ErrorMessage = "Vui lòng nhập đúng số điện thoại")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Số điện thoại chỉ được phép nhập số.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Vui lòng điền email")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn chủ đề")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập nội dung")]
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
