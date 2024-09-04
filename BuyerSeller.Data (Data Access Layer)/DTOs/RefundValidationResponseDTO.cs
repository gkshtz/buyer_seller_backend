using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyerSeller.Data__Data_Access_Layer_.DTOs
{
    public class RefundValidationResponseDTO
    {
        /// <summary>
        /// valid for refund
        /// </summary>
        public bool Status { get; set; }   
        
        /// <summary>
        /// message with false status
        /// </summary>
        public string? Message { get; set; }
    }
}
