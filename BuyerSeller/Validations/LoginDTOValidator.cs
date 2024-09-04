using BuyerSeller.Models;
using FluentValidation;

namespace BuyerSeller.Application.Validations
{
    /// <summary>
    /// Validator for LoginDTO
    /// </summary>
    public class LoginDTOValidator : AbstractValidator<LoginRequestDTO>
    {
        /// <summary>
        /// LoginDTOValidator constructor
        /// </summary>
        public LoginDTOValidator() 
        {
            RuleFor(x => x.Email).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().MinimumLength(5);
        }
    }
}
