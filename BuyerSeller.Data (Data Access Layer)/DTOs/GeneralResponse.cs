using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyerSeller.Data__Data_Access_Layer_.DTOs
{
    /// <summary>
    /// DTO for generalized response
    /// </summary>
    /// <typeparam name="TResponseType">Object of Response data to be sent</typeparam>
    public class GeneralResponse<TResponseType>
    {
        /// <summary>
        /// Http Status code
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// Status response message
        /// </summary>
        public string ResponseMessage { get; set; }
        /// <summary>
        /// Dto of information to be sent in response
        /// </summary>
        public TResponseType ResponseData { get; set; }
        /// <summary>
        /// Error message if any
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
}
