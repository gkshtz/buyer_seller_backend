using BuyerSeller.Core.Utility.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuyerSeller.Models
{
    public class PurchaseTransaction
    {
        /// <summary>
        /// Transaction ID
        /// </summary>
        [Key]
        public Guid TID { get; set; }

        /// <summary>
        /// OriginalTransactionId
        /// </summary>
        public Guid OriginalTransactionId { get; set; }
        
        /// <summary>
        /// CreatedAt
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Updated At
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// PurchasedBy
        /// </summary>
        public Guid PurchasedBy { get; set; }

        /// <summary>
        /// Transaction Type
        /// </summary>
        public TransactionType TransactionType { get; set; }
        
        /// <summary>
        /// Tranction status
        /// </summary>
        public PaymentStatus TransactionStatus { get; set; }

        /// <summary>
        /// Payment Method
        /// </summary>
        public PaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// PurchasePayment ID
        /// </summary>
        public Guid PurchasePaymentId { get; set; } = Guid.Empty;

        /// <summary>
        /// Product ID
        /// </summary>
        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
