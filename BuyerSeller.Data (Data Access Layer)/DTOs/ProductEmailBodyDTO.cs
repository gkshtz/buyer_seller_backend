using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyerSeller.Data__Data_Access_Layer_.DTOs
{
    public class ProductEmailBodyDTO
    {
        public string ProductName {  get; set; }
        public decimal Amount {  get; set; }
        public int Quantity {  get; set; }
    }
}
