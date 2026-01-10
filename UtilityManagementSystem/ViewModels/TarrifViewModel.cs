using System;
using System.ComponentModel.DataAnnotations;

namespace UtilityManagementSystem.ViewModels
{
    public class TariffViewModel : IValidatableObject
    {
        public int TariffId { get; set; }

        [Required(ErrorMessage = "Utility is required")]
        public int? UtilityId { get; set; }

        public string? UtilityName { get; set; }

        [Required(ErrorMessage = "Effective From date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Effective From")]
        public DateTime EffectiveFrom { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Effective To")]
        public DateTime? EffectiveTo { get; set; }


        [Required(ErrorMessage = "Fixed Charge is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Fixed charge must be 0 or greater")]
        [Display(Name = "Fixed Charge")]
        public decimal? FixedCharge { get; set; }

        [Required(ErrorMessage = "Slab Start is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Slab Start must be 0 or greater")]
        public decimal? SlabStart { get; set; }

        [Required(ErrorMessage = "Slab End is required")]
        [Display(Name = "Slab End")]
        public decimal? SlabEnd { get; set; }


        [Required(ErrorMessage = "Rate per unit is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Rate per unit must be greater than 0")]
        [Display(Name = "Rate Per Unit")]
        public decimal RatePerUnit { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SlabEnd.HasValue && SlabEnd <= SlabStart)
            {
                yield return new ValidationResult(
                    "Slab End must be greater than Slab Start",
                    new[] { nameof(SlabEnd) }
                );
            }

            if (EffectiveTo.HasValue && EffectiveTo < EffectiveFrom)
            {
                yield return new ValidationResult(
                    "Effective To cannot be earlier than Effective From",
                    new[] { nameof(EffectiveTo) }
                );
            }
        }
    }
}
