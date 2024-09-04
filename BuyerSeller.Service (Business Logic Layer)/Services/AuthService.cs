using BuyerSeller.Data__Data_Access_Layer_.Interfaces;
using BuyerSeller.Service__Business_Logic_Layer_.Interfaces;
using Microsoft.Extensions.Configuration;
using BuyerSeller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketBookingHub.Helpers;
using BuyerSeller.Data__Data_Access_Layer_.DTOs;
using BuyerSeller.Data__Data_Access_Layer_.UnitOfWork;
using BuyerSeller.Core.Utility.Enums;

namespace BuyerSeller.Service__Business_Logic_Layer_.Services
{
    /// <summary>
    /// Auth service
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor of Auth service class
        /// </summary>
        /// <param name="configuration">Configuration dependency</param>
        /// <param name="unitOfWork">Unit of work dependency</param>
        public AuthService(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;

        }

        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="user">User details to be added</param>
        /// <returns>Added user</returns>
        public User Add(UserDTO user)
        {
            try
            {
                var addedUser = _unitOfWork.AuthRepository.Add(user);
                _unitOfWork.save();
                return addedUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Gives a token
        /// </summary>
        /// <param name="loginRequest">Login credentials</param>
        /// <returns>Token</returns>
        public string Login(LoginRequestDTO loginRequest)
        {
            try
            {
                if (loginRequest.Email != null && loginRequest.Password != null)
                {
                    User user = _unitOfWork.AuthRepository.GetUser(loginRequest);

                    if (user != null)
                    {
                        string role;
                        if(user.UserType==UserType.buyer)
                        {
                            role = "buyer";
                        }    
                        else
                        {
                            role = "seller";
                        }

                        var jwtToken = JwtTokenService.GenerateToken(user, role, _configuration);
                        return jwtToken;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
