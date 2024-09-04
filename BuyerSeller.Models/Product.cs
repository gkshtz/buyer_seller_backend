using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuyerSeller.Models
{
    public class Product
    {
        /// <summary>
        /// Identifier
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        
        /// <summary>
        /// Active or not
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Title of product
        /// </summary>
        [MaxLength(255)]
        public string Title { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Available stock
        /// </summary>
        public int StockCount { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        [Column(TypeName = "decimal(7, 2)")]
        public decimal Price { get; set; }

        /// <summary>
        /// Selling Price
        /// </summary>
        [Column(TypeName = "decimal(7, 2)")]
        public decimal SellingPrice { get; set; }

        /// <summary>
        /// Created At
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Created By
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Last updated
        /// </summary>
        public DateTime UpdatedAt { get; set; }
        
        /// <summary>
        /// Category ID
        /// </summary>
        public int CategoryId {  get; set; }
        
        [ForeignKey("CategoryId")]
        public virtual ProductCategory ProductCategory { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Product()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = CreatedAt;
        }
    }
}
