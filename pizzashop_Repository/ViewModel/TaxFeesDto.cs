using System.ComponentModel.DataAnnotations;

namespace pizzashop_Repository.ViewModel
{
    public class TaxFeesDto : IValidatableObject
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Tax name is required.")]
        [StringLength(100, ErrorMessage = "Tax name cannot exceed 100 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Tax type is required.")]
        public bool Type { get; set; } // true = percentage, false = flat

        public decimal Percentage { get; set; }

        public decimal? TaxValue { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsDefault { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Type)
            {
                // Percentage-based
                if (TaxValue <= 0 || TaxValue > 100)
                {
                    yield return new ValidationResult(
                        "Percentage must be between 0 and 100.",
                        new[] { nameof(Percentage) }
                    );
                }
            }
            else
            {
                // Flat amount-based
                if (TaxValue == null || TaxValue <= 0)
                {
                    yield return new ValidationResult(
                        "Flat tax value must be greater than 0.",
                        new[] { nameof(TaxValue) }
                    );
                }
            }
        }
    }
}
