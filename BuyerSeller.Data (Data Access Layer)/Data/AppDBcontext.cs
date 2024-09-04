using BuyerSeller.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyerSeller.Data__Data_Access_Layer_.Data
{
    public class AppDBcontext : DbContext
    {
        /// <summary>
        /// DbContext class constructor
        /// </summary>
        /// <param name="options"></param>
        public AppDBcontext(DbContextOptions<AppDBcontext> options) : base(options)
        {

        }

        /// <summary>
        /// DbSet for Users table
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// DbSet for Products table
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// DbSet for ProductCategories table
        /// </summary>
        public DbSet<ProductCategory> ProductCategories { get; set; }

        /// <summary>
        /// Dbset for PurchaseProductInformation table
        /// </summary>
        public DbSet<PurchaseProductInformation> PurchasedProductInformation { get; set; }

        /// <summary>
        /// Dbset for PurchaseTransaction table
        /// </summary>
        public DbSet<PurchaseTransaction> PurchaseTransactions { get; set; }
        public DbSet<PurchasePaymentInformation> PurchasedPaymentInformation { get;set; }

    }
}
