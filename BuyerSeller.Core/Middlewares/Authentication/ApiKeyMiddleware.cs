using BuyerSeller.Core.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace BuyerSeller.Application.Middlewares
{
    /// <summary>
    /// ApiKeyMiddleware class
    /// </summary>
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// ApiKeyMiddleware class
        /// </summary>
        /// <param name="next">Represents the next middleware component in the pipeline</param>
        /// <param name="apiKeyValidation">IApiKeyValidation Instance</param>
        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        /// <summary>
        /// Entry point for the middleware
        /// </summary>
        /// <param name="context">HTTP Request context</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            if (string.IsNullOrWhiteSpace(context.Request.Headers[Constants.ApiKeyHeaderName]))
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }

            string userApiKey = context.Request.Headers[Constants.ApiKeyHeaderName];

            if (!IsValidApiKey(userApiKey))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }
            await _next(context);
        }

        /// <summary>
        /// Validates the API key
        /// </summary>
        /// <param name="userApiKey"></param>
        /// <returns></returns>
        public bool IsValidApiKey(string userApiKey)
        {
            if (string.IsNullOrWhiteSpace(userApiKey))
            {
                return false;
            }

            string apiKey = _configuration.GetValue<string>(Constants.ApiKeyName);

            if (apiKey == null || apiKey != userApiKey)
            {
                return false;
            }

            return true;
        }
    }
}
