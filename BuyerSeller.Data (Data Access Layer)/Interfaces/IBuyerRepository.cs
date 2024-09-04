using BuyerSeller.Data__Data_Access_Layer_.DTOs;
using BuyerSeller.Models;

namespace BuyerSeller.Data__Data_Access_Layer_.Interfaces
{
    public interface IBuyerRepository
    {
        /// <summary>
        /// Get the purchase transactions associated with the buyer
        /// </summary>
        /// <param name="userId">userId of the buyer</param>
        /// <param name="transanctionStatusFilter">filter, if any</param>
        /// <returns>List of purchase transactions</returns>
        public List<PurchaseTransaction> GetPurchases(Guid userId, string transanctionStatusFilter);
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
        /// <returns>List of products as per filter</returns>
        /// <exception cref="Exception"></exception>
        public IEnumerable<Product> GetQueryableProducts(string search, string category, int? page, int pageSize ,string sortBy,string sortOrder);

        /// <summary>
        /// Fetch product info with the specified Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Product Information</returns>
        public Product GetProductById(Guid productId);

        /// <summary>
        /// For adding the log of payment of purchased product
        /// </summary>
        /// <param name="purchasePaymentInformation"></param>
        /// <returns>Record added information</returns>
        public PurchasePaymentInformation AddPurchasePaymentInformation(PurchasePaymentInformation purchasePaymentInformation);

        /// <summary>
        /// Adding the log of transaction of purchased product
        /// </summary>
        /// <param name="purchaseTransaction">Transaction information</param>
        /// <returns>Record added information</returns>
        public PurchaseTransaction AddPurachaseTransaction(PurchaseTransaction purchaseTransaction);

        /// <summary>
        /// Changing the quatity of stock after buying or refund
        /// </summary>
        /// <param name="product">product to be bought or returned info</param>
        /// <param name="quantity">quantity to be bought or returned</param>
        public void ChangeStockQuantity(Product product, int quantity);

        /// <summary>
        /// Adding the record of successfully purchasing of product
        /// </summary>
        /// <param name="purchaseProductInformation"></param>
        public void AddPurchaseProductInformation(PurchaseProductInformation purchaseProductInformation);

        /// <summary>
        /// Get Purchase Transaction using tid
        /// </summary>
        /// <param name="tid">Id of Purchase Transaction</param>
        /// <returns>Return <see cref="PurchaseTransaction"/></returns>
        public PurchaseTransaction GetPurchaseTransaction(Guid tid);

        /// <summary>
        /// Check User is valid for Refund
        /// </summary>
        /// <param name="tid">Purchase Transaction Id</param>
        /// <returns>Return <see cref="Refu"/></returns>
        public RefundValidationResponseDTO IsValidRefundable(Guid tid, Guid userId);
    }
}
