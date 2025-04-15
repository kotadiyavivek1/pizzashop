    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;

    namespace pizzashop_Repository.ViewModel
    {
        public class AddItems
        {
            [Required(ErrorMessage = "Unit is required.")]
            public int UnitId { get; set; }

            [Required(ErrorMessage = "Name is required.")]
            [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
            public string? Name { get; set; }

            [Required(ErrorMessage = "Category Name is required.")]
            public int CategoryId { get; set; }

            [Required(ErrorMessage = "Type is required.")]
            public bool Type { get; set; }

            [Required(ErrorMessage = "Rate is required.")]
            [Range(0.01, double.MaxValue, ErrorMessage = "Rate must be greater than 0.")]
            public decimal Price { get; set; }

            [Required(ErrorMessage = "Quantity is required.")]
            [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
            public int Quantity { get; set; }
            public bool IsAvailable { get; set; }
            public bool IsDefaultTax { get; set; }
            public decimal TaxPercentage { get; set; }
            public string? ShortCode { get; set; }
            public string? Description { get; set; }
            public IFormFile? Image { get; set; }
            
            public string? ImagePath { get; set; }
            public bool Isfavourite { get; set; }

            [ValidateModifierGroups(ErrorMessage = "MinSelection cannot be greater than MaxSelection.")]
            public List<ModifiergroupDto> Modifiergroups { get; set; } = new List<ModifiergroupDto>();
        }

        public class ModifiergroupDto
        {
            public int ModifierGroupId { get; set; }

            [Range(0, int.MaxValue, ErrorMessage = "MinSelection must be 0 or greater.")]
            public int MinSelection { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "MaxSelection must be at least 1.")]
            public int MaxSelection { get; set; }

            public string? Name { get; set; }
        }
        public class ValidateModifierGroups : ValidationAttribute
        {
            protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
            {
                var model = (AddItems)validationContext.ObjectInstance;
                foreach (var group in model.Modifiergroups)
                {
                    if (group.MinSelection > group.MaxSelection)
                    {
                        return new ValidationResult(ErrorMessage);
                    }
                }
                return ValidationResult.Success;
            }
        }
    }