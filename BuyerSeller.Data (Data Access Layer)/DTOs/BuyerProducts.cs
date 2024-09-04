using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyerSeller.Data__Data_Access_Layer_.DTOs
{
    public class BuyerProducts
    {
        /// <summary>
        /// List of products that buyer can buy
        /// </summary>
        public List<BuyerProduct> Products { get; set; }
    }
}
