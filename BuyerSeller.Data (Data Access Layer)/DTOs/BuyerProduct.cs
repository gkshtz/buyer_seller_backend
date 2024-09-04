using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyerSeller.Data__Data_Access_Layer_.DTOs
{
    public class BuyerProduct
    {
        /// <summary>
        /// Product ID
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Title of product
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Product description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Price of prouct
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Category name of product
        /// </summary>
        public string CategoryName { get; set; }
    }
}
