using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyerSeller.Data__Data_Access_Layer_.DTOs
{
    public class TopProductsDTO
    {
        /// <summary>
        /// Product ID 
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Name of the product
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Description of the product
        /// </summary>
        public string ProductDescription { get; set; }

        /// <summary>
        /// Frequency of the product
        /// </summary>
        public int Frequency { get; set; }
    }
}
