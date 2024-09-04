using BuyerSeller.Data__Data_Access_Layer_.DTOs;
using BuyerSeller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyerSeller.Service__Business_Logic_Layer_.Interfaces
{
    /// <summary>
    /// Interface for Auth service
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="user">User details to be added</param>
        /// <returns>Added user</returns>
        public User Add(UserDTO user);

        /// <summary>
        /// Gives a token
        /// </summary>
        /// <param name="loginRequest">Login credentials</param>
        /// <returns>Token</returns>
        public string Login(LoginRequestDTO loginRequest);
    }
}
