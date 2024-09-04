using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyerSeller.Data__Data_Access_Layer_.DTOs
{
    public class RefundResponseDTO
    {
        public decimal Amount { get; set; }

        public Guid PurchaseTransactionId { get; set; }
    }
}
