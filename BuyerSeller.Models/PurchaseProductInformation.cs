using BuyerSeller.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuyerSeller.Models
{
    public class PurchaseProductInformation
    {
        /// <summary>
        /// Identifier
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Product ID
        /// </summary>
        public Guid ProductId { get; set; }

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
        /// Quantity purchased
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Selling Price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Seller ID of the product
        /// </summary>
        public Guid SellerId { get; set; }

        /// <summary>
        /// Created At
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// ID of purchase transaction
        /// </summary>
        public Guid PurchaseTransactionID { get; set; }
    }
}
