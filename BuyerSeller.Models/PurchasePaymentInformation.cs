using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuyerSeller.Core.Utility.Enums;
namespace BuyerSeller.Models
{
    public class PurchasePaymentInformation
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// transaction id of pending version
        /// </summary>
        public Guid OriginalTransactionId { get; set; }
        /// <summary>
        /// Cash,wallet,card,upi,etc
        /// </summary>
        public PaymentMethod PaymentMethod { get; set; }
        /// <summary>
        /// Payment pending or completed or failed
        /// </summary>
        public PaymentStatus Status { get; set; }

    }
}
