using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyerSeller.Data__Data_Access_Layer_.DTOs
{
    /// <summary>
    /// ProductDTO
    /// </summary>
    public class ProductDTO
    {
        /// <summary>
        /// Active or not
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Title of the product
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Description of the product
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Stock count
        /// </summary>
        public int? StockCount { get; set; }

        /// <summary>
        /// Price of the product
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Selling price of the product
        /// </summary>
        public decimal? SellingPrice { get; set; }

        /// <summary>
        /// Category of the product
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// ProductDTO constructor
        /// </summary>
        public ProductDTO()
        {
            IsActive = null;
            Title = null;
            Description = null;
            StockCount = null;
            Price = null;
            SellingPrice = null;
            CategoryId = null;
        }
    }
}
