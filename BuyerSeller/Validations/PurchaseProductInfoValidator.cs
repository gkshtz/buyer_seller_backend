using BuyerSeller.Data__Data_Access_Layer_.DTOs;
using FluentValidation;

namespace BuyerSeller.Application.Validations
{
    /// <summary>
    /// PurchasProductInfo validator class
    /// </summary>
    public class PurchaseProductInfoValidator : AbstractValidator<PurchaseProductInfo>
    {
        /// <summary>
        /// PuchaseProductInfo Validator constructor
        /// </summary>
        public PurchaseProductInfoValidator()
        {
            RuleFor(x=>x.ProductId).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithMessage("Product ID cannot be empty")
                .Must(value => Guid.TryParse(value.ToString(), out _))
                .WithMessage("Must be a valid GUID");

            RuleFor(x => x.PaymentMethod).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithMessage("Payment menthod cannot be empty")
                .Must(value => (int)value >= 0 && (int)value <= 4)
                .WithMessage("Please enter a valid payment method");
        }
    }
}
