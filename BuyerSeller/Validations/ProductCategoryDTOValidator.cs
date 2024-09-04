using BuyerSeller.Data__Data_Access_Layer_.DTOs;
using FluentValidation;

namespace BuyerSeller.Application.Validations
{
    /// <summary>
    /// Validator for Product Category
    /// </summary>
    public class ProductCategoryDTOValidator : AbstractValidator<ProductCategoryDTO>
    {

        public ProductCategoryDTOValidator()
        {
            RuleFor(x => x.CategoryDisplayName).Cascade(CascadeMode.StopOnFirstFailure)
                                               .NotEmpty()
                                               .WithMessage("Please Enter Category title")
                                               .Matches("^[a-zA-Z ]+$")
                                               .WithMessage("Should contain only letters");

            RuleFor(x => x.IsActive).Cascade(CascadeMode.StopOnFirstFailure)
                                    .NotEmpty()
                                    .WithMessage("Please Enter IsActive");
        }
    }
}
