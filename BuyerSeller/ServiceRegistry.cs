using BuyerSeller.Service__Business_Logic_Layer_.Interfaces;
using BuyerSeller.Service__Business_Logic_Layer_.Services;

namespace BuyerSeller.Application
{
    /// <summary>
    /// Registers Services
    /// </summary>
    public static class ServiceRegistry
    {
        /// <summary>
        /// Registers the services
        /// </summary>
        /// <param name="services">services</param>
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IBuyerService, BuyerService>();
            services.AddTransient<ISellerService, SellerService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}
