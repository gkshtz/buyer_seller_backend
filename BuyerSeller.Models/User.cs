using BuyerSeller.Core.Utility.Enums;
using System.ComponentModel.DataAnnotations;

namespace BuyerSeller.Models
{
    public class User
    {
        /// <summary>
        /// UserID
        /// </summary>
        [Key]
        public Guid UserId { get; set; }
        
        /// <summary>
        /// FirstName
        /// </summary>
        public string FirstName { get; set; }
        
        /// <summary>
        /// LastName
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// UserType
        /// </summary>
        public UserType UserType { get; set; }
        
        /// <summary>
        /// Wallet Balance
        /// </summary>
        public int WalletBalance {  get; set; }

        /// <summary>
        /// CreatedAt
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// CreatedAt
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public User()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = CreatedAt;
        }
    }
}
