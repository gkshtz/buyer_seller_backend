namespace BuyerSeller.Core.Middlewares.Exceptions
{
    /// <summary>
    /// Custom Exception class for error handling
    /// </summary>
    public class CustomException : Exception
    {
        public int StatusCode { get; set; }

        /// <summary>
        /// Create new instance of <see cref="CustomException"/>
        /// </summary>
        /// <param name="statudCode">Response status code</param>
        /// <param name="message">Error response message</param>
        public CustomException(int statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
