using BuyerSeller.Data__Data_Access_Layer_.DTOs;
using BuyerSeller.Models;

namespace BuyerSeller.Data__Data_Access_Layer_.Interfaces
{
    /// <summary>
    /// Seller Repository
    /// </summary>
    public interface ISellerRepository
    {
        /// <summary>
        /// Seller can get the list of products
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="filter">Filter</param>
        /// <param name="search">Search</param>
        /// <param name="size">Size</param>
        /// <param name="page">Page</param>
        /// <returns>List of products</returns>
        public List<Product> GetSellerProducts(Guid userId, string filter, string search, int size, int page);

        /// <summary>
        /// Seller adds the new product
        /// </summary>
        /// <param name="product">Details of the product</param>
        /// <returns>Added product details</returns>
        public Product AddProduct(Guid userId, ProductDTO product);

        /// <summary>
        /// Seller Updates the product
        /// </summary>
        /// <param name="userId">UserID of the seller</param>
        /// <param name="productId">Product ID</param>
        /// <param name="updateProduct">New Product details</param>
        /// <returns>Result in form of boolean</returns>
        public bool UpdateProduct(Guid userId, Guid productId, ProductDTO updateProduct);

        /// <summary>
        /// Seller deletes a product
        /// </summary>
        /// <param name="userId">User ID of the seller</param>
        /// <param name="productId">Product ID of the product to be deleted</param>
        /// <returns>Result in form of boolean</returns>
        public bool DeleteProduct(Guid userId, Guid productId);

        /// <summary>
        /// Get product by product IDs
        /// </summary>
        /// <param name="userId">User ID of the seller</param>
        /// <param name="productId">Product ID of the product</param>
        /// <returns>Details of the product</returns>
        public Product GetProduct(Guid userId, Guid productId);

        /// <summary>
        /// Get the list of top selling products
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="startDateTime">Start DateTime</param>
        /// <returns>List of top selling products</returns>
        List<TopProductsDTO> TopSellingProducts(Guid userId, DateTime startDateTime);

        /// <summary>
        /// Get list of Seller Purchases
        /// </summary>
        /// <param name="userId">Id of User</param>
        /// <returns>List of Seller Purchases</returns>
        public Task<List<PurchasesDTO>> PurchasesListAsync(Guid userId);

        /// <summary>
        /// Load Products data from csv
        /// </summary>
        /// <param name="csvFileStream">Stream of csv data</param>
        /// <returns>Number of records added</returns>
        public Task<int> LoadProductsFromCsvAsync(Guid userId, Stream csvFileStream);
    }
}
