using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineTicketBookingSystem.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "Họ và tên không được để trống.")]
        [MinLength(6, ErrorMessage = "Họ và tên ít nhất 6 ký tự.")]
        public string? FullName { get; set; }
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
        [Required(ErrorMessage = "Email không được để trống")]
        public string? Email { get; set; }
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*?&]*$",
            ErrorMessage = "Mật khẩu phải chứa ít nhất một chữ cái, số và kí tự đặc biệt.")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Giới tính không được để trống.")]

        public string? Gender { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [StringLength(11, MinimumLength = 10, ErrorMessage = "Vui lòng nhập đúng số điện thoại")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Số điện thoại chỉ được phép nhập số.")]
        public string? PhoneNumber { get; set; }
        public string? ProvinceCode { get; set; }
        public string? DistrictCode { get; set; }
        public string? WardCode { get; set; }

        [ForeignKey(nameof(ProvinceCode))]
        public Province? Province { get; set; }

        [ForeignKey(nameof(DistrictCode))]
        public District? District { get; set; }

        [ForeignKey(nameof(WardCode))]
        public Ward? Ward { get; set; }

        public string? Address { get; set; }
        [Required(ErrorMessage = "Ngày sinh không được để trống.")]
        public DateTime? DateOfBirth { get; set; }

        [DataType(DataType.Currency)]
        public decimal Balance { get; set; } = 0;

        public string AccountType { get; set; } = "Local";
        [Required(ErrorMessage = "Vui lòng chọn vai trò người dùng")]
        public int? GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        [ValidateNever]
        public Group? Group { get; set; }

        public string? Avatar { get; set; }

        public bool IsActive { get; set; } = false;
        public bool IsStatus { get; set; } = false;

        public string? CodeId { get; set; }
        public DateTime? CodeExpired { get; set; }
        public int LoginAttempts { get; set; } = 0;

        public string? CreatedAt { get; set; } = DateTime.Now.ToString();
        public string? UpdatedAt { get; set; }
        public string? LastLogin { get; set; }

    }
}
