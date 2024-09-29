using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using OnlineTicketBookingSystem.Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineTicketBookingSystem.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*?&]*$",
            ErrorMessage = "Mật khẩu phải chứa ít nhất một chữ cái, số và kí tự đặc biệt.")]
        public string Password { get; set; }
        public string? Gender { get; set; }
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
        public DateTime? DateOfBirth { get; set; }

        [DataType(DataType.Currency)]
        public decimal Balance { get; set; } = 0;

        public string AccountType { get; set; } = SD.AccountType_Local;
        public int? GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        [ValidateNever]
        public Group? Group { get; set; }

        public string? Avatar { get; set; }

        public bool IsActive { get; set; } = SD.IsActive_False;
        public bool IsStatus { get; set; } = SD.IsStatus_False;

        public string? CodeId { get; set; }
        public DateTime? CodeExpired { get; set; }
        public int LoginAttempts { get; set; } = 0;

        public string? CreatedAt { get; set; }
        public string? UpdatedAt { get; set; }
        public string? LastLogin { get; set; }

    }
}
