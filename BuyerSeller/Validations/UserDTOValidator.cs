using BuyerSeller.Data__Data_Access_Layer_.Data;
using BuyerSeller.Data__Data_Access_Layer_.DTOs;
using BuyerSeller.Data__Data_Access_Layer_.Interfaces;
using BuyerSeller.Data__Data_Access_Layer_.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BuyerSeller.Application.Validations
{
    /// <summary>
    /// Validator for UserDTO
    /// </summary>
    public class UserDTOValidator : AbstractValidator<UserDTO>
    {
        private IAuthRepository _authRepository;
        /// <summary>
        /// UserDTOValidator constructor
        /// </summary>
        public UserDTOValidator()
        {
            RuleFor(x => x.FirstName).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().WithMessage("Please enter the first name")
                 .Matches("^[a-zA-Z ]+$").WithMessage("Should contain only letters");

            RuleFor(x => x.LastName).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().WithMessage("Please enter the last name")
                .Matches("^[a-zA-Z ]+$").WithMessage("Should contain only digits.");

            RuleFor(x => x.Email).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Email cannot be empty")
                .EmailAddress().WithMessage("Should be a valid email address");

            RuleFor(x => x.UserType).Must(value => value == 0 || value == 1).WithMessage("User Type can only contain value 0 or 1");

            RuleFor(x => x.WalletBalance).GreaterThanOrEqualTo(0).WithMessage("Wallet balance should not be negative");
        }

        private bool IsUniqueEmail(string email)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDBcontext>();
            optionsBuilder.UseSqlServer("Server=PG02S16W\\SQLEXPRESS; Database=ECommerce; Integrated Security=true; TrustServerCertificate=true;");

            var dbcontext = new AppDBcontext(optionsBuilder.Options);

            _authRepository = new AuthRepository(dbcontext);
            return !_authRepository.CheckEmailExist(email);
        }
    }
}
