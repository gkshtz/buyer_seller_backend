using BuyerSeller.Core.Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyerSeller.Data__Data_Access_Layer_.DTOs
{
    /// <summary>
    /// Information for product to be bought
    /// </summary>
    public class PurchaseProductInfo
    {
        /// <summary>
        /// ProductId
        /// </summary>
        public Guid ProductId { get; set; }
        /// <summary>
        /// Quantity of product to be bought
        /// </summary>
        public int Quantity {  get; set; }

        /// <summary>
        /// Cash,wallet,upi,card,etc
        /// </summary>
        public PaymentMethod PaymentMethod { get; set; }
    }
}
