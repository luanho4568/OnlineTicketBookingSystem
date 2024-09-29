using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineTicketBookingSystem.Models
{
    public class District
    {
        [Key]
        public string Code { get; set; }

        public string? Name { get; set; }

        public string? NameEn { get; set; }
        public string? FullName { get; set; }
        public string? FullNameEn { get; set; }
        public string? CodeName { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(Province))]
        public string? ProvinceCode { get; set; }
        public Province? Province { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(AdministrativeUnit))]
        public int? AdministrativeUnitId { get; set; }
        public AdministrativeUnit? AdministrativeUnit { get; set; }
    }
}
