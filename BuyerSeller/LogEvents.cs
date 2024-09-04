using Microsoft.AspNetCore.Http.HttpResults;
using System.Net.NetworkInformation;

namespace BuyerSeller.Application
{
    /// <summary>
    /// Class for LogEvent IDs
    /// </summary>
    public class LogEvents
    { 
        /// <summary>
        /// NotFound Error
        /// </summary>
        public const int ItemNotFound = 4000;

        /// <summary>
        /// Unauthorized Error
        /// </summary>
        public const int Unauthorized = 4001;


        /// <summary>
        /// Forbidden Error
        /// </summary>
        public const int Forbidden = 4002;

        /// <summary>
        /// Server Error
        /// </summary>
        public const int ServerError = 4003;
    }
}
