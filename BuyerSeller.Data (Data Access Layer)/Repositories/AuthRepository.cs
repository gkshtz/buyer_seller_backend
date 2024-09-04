using BuyerSeller.Core.Utility.Enums;
using BuyerSeller.Data__Data_Access_Layer_.Data;
using BuyerSeller.Data__Data_Access_Layer_.DTOs;
using BuyerSeller.Data__Data_Access_Layer_.Interfaces;
using BuyerSeller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyerSeller.Data__Data_Access_Layer_.Repositories
{
    /// <summary>
    /// Auth Repository
    /// </summary>
    public class AuthRepository : IAuthRepository
    {
        private readonly  AppDBcontext _dbcontext;

        /// <summary>
        /// Auth Repository constructor
        /// </summary>
        /// <param name="dBcontext"></param>
        public AuthRepository(AppDBcontext dBcontext)
        {
            _dbcontext = dBcontext;
        }

        /// <summary>
        /// Add new user
        /// </summary>
        /// <param name="user">User details</param>
        /// <returns>Added user details</returns>
        public User Add(UserDTO user)
        {
            try
            {
                User addUser = new User()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = user.Password,
                    UserType = (UserType)user.UserType,
                    WalletBalance = user.WalletBalance
                };

                _dbcontext.Users.Add(addUser);
                return addUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        /// <summary>
        /// Changing the wallet balance of buyer and seller after purchasing or returned
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="balance"></param>
        public void ChangeWalletBalance(Guid userId, decimal balance)
        {
            User user= _dbcontext.Users.FirstOrDefault(x=>x.UserId==userId);
            user.WalletBalance= (int)balance;
            _dbcontext.SaveChanges();
        }

        /// <summary>
        /// Get user by username and password
        /// </summary>
        /// <param name="login">Login credentials</param>
        /// <returns>User details</returns>
        public User GetUser(LoginRequestDTO login)
        {
            try
            {
                User user = _dbcontext.Users.Where(x => x.Email == login.Email && x.Password == login.Password).FirstOrDefault();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        /// <summary>
        /// Getting the user with specified Id
        /// </summary>
        /// <param name="userId">userid of user</param>
        /// <returns>User information</returns>
        public User GetUserById(Guid userId)
        {
            return _dbcontext.Users.FirstOrDefault(x => x.UserId == userId);
        }


        /// <summary>
        /// Checks email is unique or not
        /// </summary>
        /// <param name="email">Email address</param>
        /// <returns>Result as the boolean</returns>
        public bool CheckEmailExist(string email)
        {
            return false;
            //return _dbcontext.Users.Any(x => x.Email == email);
        }
    }
}
