using BuyerSeller.Data__Data_Access_Layer_.Interfaces;
using BuyerSeller.Data__Data_Access_Layer_.Repositories;
using BuyerSeller.Data__Data_Access_Layer_.UnitOfWork;
using System.Runtime.CompilerServices;

namespace BuyerSeller.Application
{
    /// <summary>
    /// Registers Repositories and unit of work
    /// </summary>
    public static class RepositoryRegistry
    {
        /// <summary>
        /// Adds the Repositories
        /// </summary>
        /// <param name="services">services</param>
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IBuyerRepository, BuyerRepository>();
            services.AddTransient<IAuthRepository, AuthRepository>();
            services.AddTransient<ISellerRepository, SellerRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
        }
    }
}
