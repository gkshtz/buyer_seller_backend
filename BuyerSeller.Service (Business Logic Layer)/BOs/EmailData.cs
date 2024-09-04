using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyerSeller.Service__Business_Logic_Layer_.BOs
{
    /// <summary>
    /// DTO for Email Service
    /// </summary>
    public class EmailData
    {
        /// <summary>
        /// name of Product
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Quantity of Product
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Total Amount
        /// </summary>
        public decimal Price { get; set; }
    }
}
