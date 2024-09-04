using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyerSeller.Data__Data_Access_Layer_.DTOs
{
    public class PurchasesDTO
    {
        public Guid TransactionId { get; set; }

        public Guid ProductId { get; set; }
        
        public string ProductTitle {  get; set; }

        public string ProductDescription { get; set; }

        public int Quantity {  get; set; }

        public decimal TotalPrice { get; set; }
    }
}
