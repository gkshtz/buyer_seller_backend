using BuyerSeller.Data__Data_Access_Layer_.DTOs;
using BuyerSeller.Models;

namespace BuyerSeller.Service__Business_Logic_Layer_.Interfaces
{
    /// <summary>
    /// Interface for buyer service
    /// </summary>
    public interface IBuyerService
    {
        /// <summary>
        /// Get the list of purchases
        /// </summary>
        /// <param name="userId">Id of the buyer</param>
        /// <param name="transanctionStatusFilter">Transaction status Filter</param>
        /// <returns>List of purchase transactions</returns>
        public List<PurchaseTransaction> GetPurchases(Guid userId, string transanctionStatusFilter);



        /// <summary>
        /// List of all the products
        /// </summary>
        /// <returns>List of all the products</returns>
        public BuyerProducts GetAllProducts();


        /// <summary>
        /// Products for buyer with filter
        /// </summary>
        /// <param name="search">Search by name or description or keyword</param>
        /// <param name="category">Search by category</param>
        /// <param name="page">Pagination page number</param>
        /// <param name="pageSize">Pagination page size</param>
        /// <param name="sortBy">Sort by title or price</param>
        /// <param name="sortOrder">Asc or desc</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IEnumerable<BuyerProduct> GetProducts(string search, string category, int? page, int pageSize,string sortBy,string sortOrder);


        /// <summary>
        /// Implementation of purchasing the product and creating the transaction
        /// </summary>
        /// <param name="userId">userId of buyer</param>
        /// <param name="purchaseProductInfo">DTO for product information to be bought</param>
        /// <returns>Response with Purhchased product information</returns>
        /// <exception cref="Exception"></exception>
        public GeneralResponse<PurchaseProductInformation> PurchaseProduct(Guid userId, PurchaseProductInfo purchaseProductInfo);

        /// <summary>
        /// Get Transaction refund 
        /// </summary>
        /// <param name="id">Transaction Id</param>
        /// <returns>Refund Details</returns>
        public GeneralResponse<RefundResponseDTO> Refund(Guid id, Guid userId);
    }
}
