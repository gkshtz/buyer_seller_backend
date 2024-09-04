using BuyerSeller.Data__Data_Access_Layer_.Interfaces;
using BuyerSeller.Models;

using BuyerSeller.Data__Data_Access_Layer_.Data;
using BuyerSeller.Data__Data_Access_Layer_.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using BuyerSeller.Core.Utility.Enums;

namespace BuyerSeller.Data__Data_Access_Layer_.Repositories
{
    /// <summary>
    /// Buyer Repository
    /// </summary>
    public class BuyerRepository : IBuyerRepository
    {
        private readonly AppDBcontext _dbcontext;

        /// <summary>
        /// Constructor for buyer repository
        /// </summary>
        /// <param name="dbontext">AppDBcontect dependency</param>
        public BuyerRepository(AppDBcontext dbontext)
        {
            _dbcontext = dbontext;
        }

        public PurchaseTransaction AddPurachaseTransaction(PurchaseTransaction purchaseTransaction)
        {
            _dbcontext.PurchaseTransactions.Add(purchaseTransaction);
            _dbcontext.SaveChanges();
            return purchaseTransaction;
        }

        public PurchasePaymentInformation AddPurchasePaymentInformation(PurchasePaymentInformation purchasePaymentInformation)
        {
            _dbcontext.PurchasedPaymentInformation.Add(purchasePaymentInformation);
            _dbcontext.SaveChanges();
            return purchasePaymentInformation; 
        }

        public void AddPurchaseProductInformation(PurchaseProductInformation purchaseProductInformation)
        {
            _dbcontext.PurchasedProductInformation.Add(purchaseProductInformation);
            _dbcontext.SaveChanges();
        }

        public void ChangeStockQuantity(Product product, int quantity)
        {

            Product product1 = _dbcontext.Products.SingleOrDefault(x=>x.Id == product.Id);
            product1.StockCount= quantity;
            _dbcontext.SaveChanges();
        }

        public BuyerProducts GetAllProducts()
        {
            List<Product> products = _dbcontext.Products.Include(product => product.ProductCategory).ToList();            List<BuyerProduct> buyerProducts = new List<BuyerProduct>();
            foreach (Product product in products) { 
                BuyerProduct buyerProduct = new BuyerProduct()
                {
                    Title = product.Title,
                    Description = product.Description,
                    Price = product.SellingPrice,
                    CategoryName=product.ProductCategory.CategoryDisplayName
                };
                buyerProducts.Add(buyerProduct);
            }
            BuyerProducts productsToBuyer= new BuyerProducts()
            {
                Products=buyerProducts
            };
            return productsToBuyer;
        }

        public Product GetProductById(Guid productId)
        {
            return _dbcontext.Products.FirstOrDefault(x => x.Id == productId);
        }

        /// <summary>
        /// Get Purchase transaction using tid
        /// </summary>
        /// <param name="tid">Purchase Trasaction Id</param>
        /// <returns>Return <see cref="PurchaseTransaction"/></returns>
        public PurchaseTransaction GetPurchaseTransaction(Guid tid)
        {
             return _dbcontext.PurchaseTransactions.FirstOrDefault(x => x.TID == tid);
        }

        /// <summary>
        /// Check Transaction is refundable
        /// </summary>
        /// <param name="tid">purchase transaction id</param>
        /// <returns>Return <see cref="RefundValidationResponseDTO"/></returns>
        public RefundValidationResponseDTO IsValidRefundable(Guid tid, Guid userId)
        {
            //Get OriginalTransactionId using TID
            PurchaseTransaction purchaseTransaction = _dbcontext.PurchaseTransactions.FirstOrDefault(pt => pt.TID == tid);
            Guid OriginalTransactionId = purchaseTransaction.OriginalTransactionId;
            Guid purchasedBy = purchaseTransaction.PurchasedBy;

            if(purchasedBy != userId)
            {
                return new RefundValidationResponseDTO()
                {
                    Status = false,
                    Message = "Invalid Request"
                };
            }

            //Get first pending transaction using above OriginalTransactionId
            PurchaseTransaction purchaseFirstTransaction = _dbcontext.PurchaseTransactions
                                                           .FirstOrDefault(pt => (
                                                                (pt.OriginalTransactionId == OriginalTransactionId) &&
                                                                (pt.TransactionType == TransactionType.purchase) && 
                                                                (pt.TransactionStatus == PaymentStatus.completed)
                                                           ));
            //Check refund is possible
            DateTime refundThresholdDate = purchaseFirstTransaction.CreatedAt.Date.AddDays(3);
            if( refundThresholdDate < DateTime.UtcNow.Date)
            {
                return new RefundValidationResponseDTO()
                {
                    Status = false,
                    Message = "Refund is valid only for three days."
                };
            }

            //Fetch refund transaction that have OriginalTransactionId equal to above OriginalTransactionId
            PurchaseTransaction successfulRefundTransaction = _dbcontext.PurchaseTransactions
                                                           .FirstOrDefault(pt => (
                                                                (pt.OriginalTransactionId == OriginalTransactionId) &&
                                                                (pt.TransactionType == TransactionType.refund) && 
                                                                (pt.TransactionStatus == PaymentStatus.completed)
                                                            ));


            //Check refund is performed already or not
            if(successfulRefundTransaction != null )
            {
                return new RefundValidationResponseDTO()
                {
                    Status = false,
                    Message = "Transaction is already refunded."
                };
            }

            return new RefundValidationResponseDTO()
            {
                Status = true,
            };
        }

        /// <summary>
        /// Get the purchase transactions associated with the buyer
        /// </summary>
        /// <param name="userId">userId of the buyer</param>
        /// <param name="transanctionStatusFilter">filter, if any</param>
        /// <returns>List of purchase transactions</returns>
        public List<PurchaseTransaction> GetPurchases(Guid userId, string transanctionStatusFilter)
        {
            try
            {
                List<PurchaseTransaction> purchaseTransactions = _dbcontext.PurchaseTransactions.Where(x=>x.PurchasedBy==userId).ToList();

                if (!string.IsNullOrEmpty(transanctionStatusFilter))
                {
                    transanctionStatusFilter = transanctionStatusFilter.ToLower();
                    purchaseTransactions = purchaseTransactions.Where(x => x.TransactionStatus.ToString() == transanctionStatusFilter).ToList();
                }

                //Order the transactions according to the date.
                purchaseTransactions = purchaseTransactions.OrderByDescending(x => x.CreatedAt).ToList();
                return purchaseTransactions;
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.ToString());
            }
        }
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
        public IEnumerable<Product> GetQueryableProducts(string search, string category, int? page, int pageSize, string sortBy,string sortOrder)
        {
            var products = _dbcontext.Products.Include(x=>x.ProductCategory).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Title.Contains(search) || p.Description.Contains(search));
            }

            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.ProductCategory.CategoryDisplayName == category);
            }

            if (page.HasValue)
            {
                products = products.Skip((page.Value - 1) * pageSize).Take(pageSize);
            }

            if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(sortOrder))
            {
                switch (sortBy.ToLower())
                {
                    case "title":
                        products = sortOrder.ToLower() == "asc"
                            ? products.OrderBy(p => p.Title)
                            : products.OrderByDescending(p => p.Title);
                        break;
                    case "price":
                        products = sortOrder.ToLower() == "asc"
                            ? products.OrderBy(p => p.Price)
                            : products.OrderByDescending(p => p.Price);
                        break;
                   
                }
            }


            return products.ToList();
        }
    }
}
