using BuyerSeller.Data__Data_Access_Layer_.DTOs;
using FluentValidation;

namespace BuyerSeller.Application.Validations
{
    /// <summary>
    /// Validator for productDTO
    /// </summary>
    public class ProductDTOValidator : AbstractValidator<ProductDTO>
    {
        /// <summary>
        /// ProductDTOValidator constructor
        /// </summary>
        public ProductDTOValidator()
        {
            RuleFor(x => x.Title).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().WithMessage("Please enter the product title")
                 .Matches("^[a-zA-Z ]+$").WithMessage("Should contain only letters");

            RuleFor(x => x.Description).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().WithMessage("Please enter the product description")
                 .Matches("^[a-zA-Z ]+$").WithMessage("Should contain only letters");

            RuleFor(x => x.Price).GreaterThanOrEqualTo(0)
                .WithMessage("Price cannot be negative");
            
            RuleFor(x=>x.SellingPrice).GreaterThanOrEqualTo(0)
                .WithMessage("Selling price cannot be negative");

            RuleFor(x => x.StockCount).GreaterThanOrEqualTo(0)
                .WithMessage("Stock count cannot be negative");
        }
    }
}
