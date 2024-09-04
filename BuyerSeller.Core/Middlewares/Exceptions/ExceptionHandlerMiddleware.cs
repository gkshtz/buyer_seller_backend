using BuyerSeller.Core.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BuyerSeller.Core.Middlewares.Exceptions
{
    /// <summary>
    /// Custom Exception-Handler middleware at global scope
    /// </summary>
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        /// <summary>
        /// Creates new instance of the middleware
        /// </summary>
        /// <param name="next">RequestHandler to call next request-handler in middleware pipeline</param>
        /// <param name="env">host environment</param>
        public ExceptionHandlerMiddleware(RequestDelegate next, IHostEnvironment env, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _env = env;
            _logger = logger;
        }

        /// <summary>
        /// Exception handling logic
        /// </summary>
        /// <param name="context">Instance of <see cref="HttpContext"/></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CustomException ex)
            {
                await SendErrorAsync(context, ex.StatusCode, ex.Message);
                _logger.LogInformation("[{method}]: {path}", context.Request.Method, context.Request.Path);
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                {
                    await SendErrorAsync(context, StatusCodes.Status500InternalServerError, ex.Message);
                }
                else
                {
                    await SendErrorAsync(context, StatusCodes.Status500InternalServerError, Messages.SERVER_ERROR);
                }

                _logger.LogError("Error: [{method}] {path} \nError: {error}", context.Request.Method, context.Request.Path, ex.StackTrace);
            }
        }

        private async Task SendErrorAsync(HttpContext context, int statusCode, string message)
        {
            var errorResponse = new ErrorResponse
            {
                StatusCode = statusCode,
                ErrorMessage = message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = errorResponse.StatusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }

    /// <summary>
    /// static extension class for registering <see cref="ExceptionHandlerMiddleware"/>
    /// </summary>
    public static class ExpceptionHandlerMiddlewareExtensions
    {
        /// <summary>
        /// Register <see cref="ExceptionHandlerMiddleware"/> middleware
        /// </summary>
        /// <param name="builder">Instance of <see cref="IApplicationBuilder"/></param>
        /// <returns>Reference of <see cref="IApplicationBuilder"/></returns>
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }

    /// <summary>
    /// Error response data
    /// </summary>
    internal record struct ErrorResponse
    {
        /// <summary>
        /// Error http-status code
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Error message
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
