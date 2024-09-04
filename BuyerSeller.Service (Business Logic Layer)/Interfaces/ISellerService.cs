using BuyerSeller.Data__Data_Access_Layer_.DTOs;
using BuyerSeller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BuyerSeller.Service__Business_Logic_Layer_.Interfaces
{
    /// <summary>
    /// Interface for seller service
    /// </summary>
    public interface ISellerService
    {
        /// <summary>
        /// Seller can get the list of products
        /// </summary>
        /// <param name="userId">User ID of the seller</param>
        /// <param name="filter">Fiter by category ID</param>
        /// <param name="search">Search</param>
        /// <param name="size">size per page</param>
        /// <param name="page">page</param>
        /// <returns>List of products</returns>
        public List<Product> GetSellerProducts(Guid userId, string filter, string search, int size, int page);

        /// <summary>
        /// Add a new product
        /// </summary>
        /// <param name="userId">User ID of the seller</param>
        /// <param name="product">Product details to be added</param>
        /// <returns>Added product</returns>
        public Product AddProduct(Guid userId, ProductDTO product);

        /// <summary>
        /// Updates the product
        /// </summary>
        /// <param name="userId">User ID of the seller</param>
        /// <param name="productId">Product ID</param>
        /// <param name="updateProduct">Product details to be updated</param>
        /// <returns>Result in the form of boolean</returns>
        public bool UpdateProduct(Guid userId, Guid productId, ProductDTO updateProduct);

        /// <summary>
        /// Seller can delete a product
        /// </summary>
        /// <param name="userId">User ID of the seller</param>
        /// <param name="productId">Id of the product to be deleted</param>
        /// <returns>Result in the form of boolean</returns>
        public bool DeleteProduct(Guid userId, Guid productId);

        /// <summary>
        /// Get the details of product by ID
        /// </summary>
        /// <param name="userId">ID of the seller</param>
        /// <param name="productId">Id of the product</param>
        /// <returns>Product details</returns>
        public Product GetProduct(Guid userId, Guid productId);

        /// <summary>
        /// Get the top selling products list
        /// </summary>
        /// <param name="userId">ID of the seller</param>
        /// <param name="duration">Duration like days, weeks, months</param>
        /// <param name="number">Number of days, weeks, months</param>
        /// <returns>List of top selling products</returns>
        public List<TopProductsDTO> TopSellingProducts(Guid userId, string duration, int number);

        /// <summary>
        /// Get start date from which top selling products to be searched
        /// </summary>
        /// <param name="currentDateTime">Current Datetime</param>
        /// <param name="duration">Duration</param>
        /// <param name="number">Number of days, weeks, months</param>
        /// <returns>Start DateTime</returns>
        public DateTime GetStartDateTime(DateTime currentDateTime, string duration, int number);

        /// <summary>
        /// Get List of Seller Purchases
        /// </summary>
        /// <param name="userId">Id of User</param>
        /// <returns>List of Purchases</returns>
        public Task<List<PurchasesDTO>> PurchasesListAsync(Guid userId);

        /// <summary>
        /// Load products data from Csv
        /// </summary>
        /// <param name="csvStream">Csv file-stream</param>
        /// <param name="userInfo">User data from token</param>
        /// <returns>Number of products added</returns>
        public Task<int> LoadProductsDataFromCsvAsync(Stream csvStream, ClaimsPrincipal userInfo);
    }
}
