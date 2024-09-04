using BuyerSeller.Core.Utility.Enums;
using BuyerSeller.Data__Data_Access_Layer_.Data;
using BuyerSeller.Data__Data_Access_Layer_.DTOs;
using BuyerSeller.Data__Data_Access_Layer_.Interfaces;
using BuyerSeller.Models;
using CsvHelper;
using Microsoft.EntityFrameworkCore;

namespace BuyerSeller.Data__Data_Access_Layer_.Repositories
{
    /// <summary>
    /// Seller Repository
    /// </summary>
    public class SellerRepository : ISellerRepository
    {
        private readonly AppDBcontext _dbcontext;
        
        /// <summary>
        /// Seller Repository constructor
        /// </summary>
        /// <param name="dbcontext">AppDBcontext Dependency</param>
        public SellerRepository(AppDBcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        /// <summary>
        /// Seller adds the new product
        /// </summary>
        /// <param name="product">Details of the product</param>
        /// <returns>Added product details</returns>
        public Product AddProduct(Guid userId, ProductDTO product)
        {
            try
            {
                bool isExist = _dbcontext.ProductCategories.Any(x => x.CategoryId == product.CategoryId && x.IsActive == true);
                
                if (!isExist)
                {
                    return null;
                }

                Product addProduct = new Product()
                {
                    IsActive = (bool) product.IsActive,
                    Title = (string) product.Title,
                    Description = (string) product.Description,
                    StockCount = (int) product.StockCount,
                    Price = (decimal) product.Price,
                    SellingPrice = (decimal) product.SellingPrice,
                    CreatedBy = userId,
                    CategoryId = (int) product.CategoryId
                };

                _dbcontext.Products.Add(addProduct);
                return addProduct;
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Seller deletes a product
        /// </summary>
        /// <param name="userId">User ID of the seller</param>
        /// <param name="productId">Product ID of the product to be deleted</param>
        /// <returns>Result in form of boolean</returns>
        public bool DeleteProduct(Guid userId, Guid productId)
        {
            try
            {
                Product product = _dbcontext.Products.Where(x => x.CreatedBy == userId && x.Id == productId).FirstOrDefault();
                if (product == null)
                {
                    return false;
                }
                _dbcontext.Products.Remove(product);

                return true;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.ToString());
            }
        }
                                        
        /// <summary>
        /// Get product by product IDs
        /// </summary>
        /// <param name="userId">User ID of the seller</param>
        /// <param name="productId">Product ID of the product</param>
        /// <returns>Details of the product</returns>
        public Product GetProduct(Guid userId, Guid productId)
        {
            try
            {
                Product product = _dbcontext.Products.Where(x => x.CreatedBy == userId && x.Id == productId).FirstOrDefault();
                
                if(product==null)
                {
                    return null;               
                }

                return product;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Seller can get the list of products
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="filter">Filter</param>
        /// <param name="search">Search</param>
        /// <param name="size">Size</param>
        /// <param name="page">Page</param>
        /// <returns>List of products</returns>
        public List<Product> GetSellerProducts(Guid userId, string filter, string search, int size, int page)
        {
            try
            {
                List<Product> products = _dbcontext.Products.Where(x => x.CreatedBy == userId).ToList();

                if (!string.IsNullOrEmpty(filter))
                {
                    int categoryID = _dbcontext.ProductCategories.Where(x => x.CategoryDisplayName.ToLower() == filter.ToLower()).Select(x => x.CategoryId).FirstOrDefault();
                    products = products.Where(x => x.CategoryId == categoryID).ToList(); 
                }

                if (!string.IsNullOrEmpty(search))
                {
                    products = products.Where(x => x.Title.ToLower() == search.ToLower()).ToList();
                }

                products = products.Skip((page - 1) * size).Take(size).ToList();

                return products;
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Get the list of top selling products
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="startDateTime">Start DateTime</param>
        /// <returns>List of top selling products</returns>
        public List<TopProductsDTO> TopSellingProducts(Guid userId, DateTime startDateTime)
        {
            try
            {
                List<TopProductsDTO> topProducts = _dbcontext.PurchasedProductInformation
                                                  .Where(x => x.SellerId == userId && x.CreatedAt >= startDateTime)
                                                  .GroupBy(x => x.ProductId)
                                                  .OrderByDescending(group => group.Count())
                                                  .Select(group => new TopProductsDTO
                                                  {
                                                      ProductId = group.Key,             // ProductId of the group
                                                      Frequency = group.Count(),        // Frequency of elements in the group
                                                      ProductName = group.First().Title,       //Product title
                                                      ProductDescription = group.First().Description    //Product description
                                                  })
                                                  .ToList();
                return topProducts;
            }
            catch(Exception ex) 
            {
                throw new Exception (ex.ToString());
            }
        }

        /// <summary>
        /// Seller Updates the product
        /// </summary>
        /// <param name="userId">UserID of the seller</param>
        /// <param name="productId">Product ID</param>
        /// <param name="updateProduct">New Product details</param>
        /// <returns>Result in form of boolean</returns>
        public bool UpdateProduct(Guid userId, Guid productId, ProductDTO updateProduct)
        {
            try
            {
                Product product = _dbcontext.Products.Where(x => x.CreatedBy == userId && x.Id == productId).FirstOrDefault();

                if (product == null)
                {
                    return false;
                }

                if(updateProduct.IsActive!=null)
                {
                    product.IsActive = (bool)updateProduct.IsActive;
                }
                
                if(updateProduct.Title!=null)
                {
                    product.Title = (string)updateProduct.Title;
                }

                if (updateProduct.Description!=null)
                {
                    product.Description = (string)updateProduct.Description;
                }
                
                if (updateProduct.StockCount!=null)
                {
                    product.StockCount = (int)updateProduct.StockCount;  
                }
                
                if (updateProduct.Price != null) 
                {
                    product.Price = (decimal)updateProduct.Price;
                }

                if (updateProduct.SellingPrice != null)
                {
                    product.SellingPrice = (decimal)updateProduct.SellingPrice;
                }
                
                if (updateProduct.CategoryId!=null)
                {
                    product.CategoryId = (int)updateProduct.CategoryId;
                }
                
                product.UpdatedAt = DateTime.UtcNow;

                _dbcontext.Products.Update(product);

                return true;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Load Products data from csv to add
        /// </summary>
        /// <param name="csvFileStream">Stream of csv data</param>
        /// <returns>Number of records added</returns>
        public async Task<int> LoadProductsFromCsvAsync(Guid userId, Stream csvFileStream)
        {
            int recordsCount = 0;

            using (var reader = new StreamReader(csvFileStream))
            using (var csvReader = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                var productList = csvReader.GetRecords<ProductDTO>()
                    .Select(productData => new Product()
                    {
                        IsActive = (bool)productData.IsActive,
                        Title = (string)productData.Title,
                        Description = (string)productData.Description,
                        StockCount = (int)productData.StockCount,
                        Price = (decimal)productData.Price,
                        SellingPrice = (decimal)productData.SellingPrice,
                        CreatedBy = userId,
                        CategoryId = (int)productData.CategoryId
                    })
                    .ToList();

                recordsCount = productList.Count;
                foreach (var product in productList)
                {
                    _dbcontext.Products.Add(product);
                }
            }

            return recordsCount;
        }

        /// <summary>
        /// Get List of Seller Purchases
        /// </summary>
        /// <param name="userId">Id of Seller</param>
        /// <returns>List of Seller</returns>
        public async Task<List<PurchasesDTO>> PurchasesListAsync(Guid userId)
        {
            List<PurchasesDTO> purchasesListWithRefundedTransaction = await _dbcontext.PurchasedProductInformation
                                                   .Where(pi => pi.SellerId == userId)
                                                   .Select(group => new PurchasesDTO
                                                   {
                                                       TransactionId = group.PurchaseTransactionID,
                                                       ProductId = group.ProductId,
                                                       ProductTitle = group.Title,
                                                       ProductDescription = group.Description,
                                                       Quantity = group.Quantity,
                                                       TotalPrice = group.Price
                                                   })
                                                   .ToListAsync();

            List<PurchaseTransaction> refundTransactionList = new List<PurchaseTransaction>();
            List<PurchasesDTO> purchases=new List<PurchasesDTO>();
            foreach (var item in purchasesListWithRefundedTransaction)
            {
                var originalTransactionId = _dbcontext.PurchaseTransactions
                                        .FirstOrDefault(innerPt => innerPt.TID == item.TransactionId).OriginalTransactionId;

                var purchaseProductInfoList = _dbcontext.PurchaseTransactions
                                        .Where(pt => pt.OriginalTransactionId == originalTransactionId)
                                        .ToList();
                bool isRefundSuccess = false;
                foreach(var purchaseTransaction in purchaseProductInfoList)
                {
                    if (purchaseTransaction.TransactionType.Equals(TransactionType.refund) && purchaseTransaction.TransactionStatus.Equals(PaymentStatus.completed)){
                        isRefundSuccess = true;
                        break;
                    }
                }
                if (!isRefundSuccess) {
                    purchases.Add(item);
                }
            }

           
            
            return purchases;

        }
    }
}
