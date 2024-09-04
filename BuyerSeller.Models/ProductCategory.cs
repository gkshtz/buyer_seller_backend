using System.ComponentModel.DataAnnotations;

namespace BuyerSeller.Models
{
    public class ProductCategory
    {
        /// <summary>
        /// Category Id
        /// </summary>
        [Key]
        public int CategoryId { get; set; }

        /// <summary>
        /// Category Name
        /// </summary>
        public string CategoryDisplayName { get; set; }
        
        /// <summary>
        /// Active or not
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Created Date and time
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Updated Date and time
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductCategory()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = CreatedAt;
        }  
    }
}
