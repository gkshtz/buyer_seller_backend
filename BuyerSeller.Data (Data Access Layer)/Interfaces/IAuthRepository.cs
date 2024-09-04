using BuyerSeller.Data__Data_Access_Layer_.DTOs;
using BuyerSeller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyerSeller.Data__Data_Access_Layer_.Interfaces
{
    /// <summary>
    /// Interface for AuthRepository
    /// </summary>
    public interface IAuthRepository
    {
        /// <summary>
        /// Add new user
        /// </summary>
        /// <param name="user">User details</param>
        /// <returns>Added user details</returns>
        User Add(UserDTO user);

        /// <summary>
        /// Add new user
        /// </summary>
        /// <param name="user">User details</param>
        /// <returns>Added user details</returns>
        User GetUser(LoginRequestDTO login);

        /// <summary>
        /// Getting the user with specified Id
        /// </summary>
        /// <param name="userId">userid of user</param>
        /// <returns>User information</returns>
        User GetUserById(Guid userId);

        /// <summary>
        /// Changing the wallet balance of buyer and seller after purchasing or returned
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="balance"></param>
        void ChangeWalletBalance(Guid userId, decimal balance);

        /// <summary>
        /// Checks email is unique or not
        /// </summary>
        /// <param name="email">Email address</param>
        /// <returns>Result as the boolean</returns>
        public bool CheckEmailExist(string email);
    }
}
