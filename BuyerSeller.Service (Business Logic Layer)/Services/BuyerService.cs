using BuyerSeller.Data__Data_Access_Layer_.Interfaces;
using BuyerSeller.Service__Business_Logic_Layer_.Interfaces;
using BuyerSeller.Models;
using BuyerSeller.Data__Data_Access_Layer_.DTOs;
using System.Transactions;
using BuyerSeller.Core.Utility.Enums;
using BuyerSeller.Data__Data_Access_Layer_.UnitOfWork;
using System.Text.Json.Nodes;
using System.Text.Json;
using BuyerSeller.Service__Business_Logic_Layer_.BOs;

namespace BuyerSeller.Service__Business_Logic_Layer_.Services
{
    /// <summary>
    /// Seller for buyer service
    /// </summary>
    public class BuyerService : IBuyerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Constructor of buyer service
        /// </summary>
        /// <param name="unitOfWork">Unit of work dependency</param>
        public BuyerService(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }
        /// <summary>
        /// List of all the products
        /// </summary>
        /// <returns>List of all the products</returns>
        public BuyerProducts GetAllProducts()
        {
            BuyerProducts products = _unitOfWork.BuyerRepository.GetAllProducts();
            return products;
        }

        /// <summary>
        /// Get the list of purchases
        /// </summary>
        /// <param name="userId">Id of the buyer</param>
        /// <param name="transanctionStatusFilter">Transaction status Filter</param>
        /// <returns>List of purchase transactions</returns>
        public List<PurchaseTransaction> GetPurchases(Guid userId, string transanctionStatusFilter)
        {
            try
            {
                List<PurchaseTransaction> purchaseTransactions = _unitOfWork.BuyerRepository.GetPurchases(userId, transanctionStatusFilter);
                return purchaseTransactions;
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.ToString());
            }
        }
        /// <summary>
        /// Implementation of purchasing the product and creating the transaction
        /// </summary>
        /// <param name="userId">userId of buyer</param>
        /// <param name="purchaseProductInfo">DTO for product information to be bought</param>
        /// <returns>Response with Purhchased product information</returns>
        /// <exception cref="Exception"></exception>
        public GeneralResponse<PurchaseProductInformation> PurchaseProduct(Guid userId, PurchaseProductInfo purchaseProductInfo)
        {
            try
            {
                Product product = _unitOfWork.BuyerRepository.GetProductById(purchaseProductInfo.ProductId);
                User user = _unitOfWork.AuthRepository.GetUserById(userId);

                if (product == null)
                {
                    GeneralResponse<PurchaseProductInformation> response = new GeneralResponse<PurchaseProductInformation>()
                    {
                        StatusCode=400,
                        ResponseMessage="Error Encountered",
                        ResponseData=null,
                        ErrorMessage="Product Not Found"
                    };
                    return response;
                }

                if (!product.IsActive)
                {
                    GeneralResponse<PurchaseProductInformation> response = new GeneralResponse<PurchaseProductInformation>()
                    {
                        StatusCode = 400,
                        ResponseMessage = "Error Encountered",
                        ResponseData = null,
                        ErrorMessage = "Product Inactive"
                    };
                    return response;
                }
                if (purchaseProductInfo.Quantity <= 0)
                {
                    GeneralResponse<PurchaseProductInformation> response = new GeneralResponse<PurchaseProductInformation>()
                    {
                        StatusCode = 400,
                        ResponseMessage = "Error Encountered",
                        ResponseData = null,
                        ErrorMessage = "Enter Correct Quantity"
                    };
                    return response;
                }
                if (!(purchaseProductInfo.PaymentMethod.Equals(PaymentMethod.cash) || purchaseProductInfo.PaymentMethod.Equals(PaymentMethod.wallet) || purchaseProductInfo.PaymentMethod.Equals(PaymentMethod.upi) || purchaseProductInfo.PaymentMethod.Equals(PaymentMethod.card) || purchaseProductInfo.PaymentMethod.Equals(PaymentMethod.netBanking)))
                {
                    GeneralResponse<PurchaseProductInformation> response = new GeneralResponse<PurchaseProductInformation>()
                    {
                        StatusCode = 400,
                        ResponseMessage = "Error Encountered",
                        ResponseData = null,
                        ErrorMessage = "Enter Correct Payment method"
                    };
                    return response;
                }
                if (product.StockCount < purchaseProductInfo.Quantity)
                {
                    GeneralResponse<PurchaseProductInformation> response = new GeneralResponse<PurchaseProductInformation>()
                    {
                        StatusCode = 400,
                        ResponseMessage = "Error Encountered",
                        ResponseData = null,
                        ErrorMessage = "Insufficient Stock quantity"
                    };
                    return response;
                }
                decimal amount=purchaseProductInfo.Quantity*product.SellingPrice;
                if (user.WalletBalance < amount)
                {
                    GeneralResponse<PurchaseProductInformation> response = new GeneralResponse<PurchaseProductInformation>()
                    {
                        StatusCode = 400,
                        ResponseMessage = "Error Encountered",
                        ResponseData = null,
                        ErrorMessage = "Insufficient Balance"
                    };
                    return response;
                }
                Guid originalTransactionId = Guid.NewGuid();
                PurchasePaymentInformation purchasePaymentInformation = new PurchasePaymentInformation()
                {
                    OriginalTransactionId = originalTransactionId,
                    PaymentMethod = purchaseProductInfo.PaymentMethod,
                    Status = PaymentStatus.pending
                }; 
                
                purchasePaymentInformation= _unitOfWork.BuyerRepository.AddPurchasePaymentInformation(purchasePaymentInformation);
                PurchaseTransaction transaction = new PurchaseTransaction()
                {
                    OriginalTransactionId=originalTransactionId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Amount = amount,
                    PurchasedBy=userId,
                    TransactionType=TransactionType.purchase,
                    PaymentMethod=purchaseProductInfo.PaymentMethod,
                    TransactionStatus=PaymentStatus.pending,
                    PurchasePaymentId=purchasePaymentInformation.Id,
                    ProductId=purchaseProductInfo.ProductId,
                };
                _unitOfWork.BuyerRepository.AddPurachaseTransaction(transaction);
                
                bool isSuccess = IsPaymentSuccess();
                if (isSuccess)
                {


                    PurchasePaymentInformation purchasePaymentInformationSuccess = new PurchasePaymentInformation()
                    {
                        OriginalTransactionId = originalTransactionId,
                        PaymentMethod = purchaseProductInfo.PaymentMethod,
                        Status = PaymentStatus.completed
                    };
                    if(purchaseProductInfo.PaymentMethod.Equals(PaymentMethod.wallet)) {
                        decimal remainingBalance = user.WalletBalance - amount;
                        _unitOfWork.AuthRepository.ChangeWalletBalance(userId, remainingBalance);
                        
                        
                    }

                    
                    Guid sellerUserId = product.CreatedBy;
                    User seller = _unitOfWork.AuthRepository.GetUserById(sellerUserId);
                    Decimal sellerRemainingAmount = seller.WalletBalance+amount;
                    _unitOfWork.AuthRepository.ChangeWalletBalance(sellerUserId, sellerRemainingAmount);

                    int remainingStockCount=product.StockCount-purchaseProductInfo.Quantity;
                    _unitOfWork.BuyerRepository.ChangeStockQuantity(product, remainingStockCount);

                    purchasePaymentInformationSuccess = _unitOfWork.BuyerRepository.AddPurchasePaymentInformation(purchasePaymentInformationSuccess);
                    PurchaseTransaction transactionSuccess = new PurchaseTransaction()
                    {
                        OriginalTransactionId = originalTransactionId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Amount = amount,
                        PurchasedBy = userId,
                        TransactionType = TransactionType.purchase,
                        PaymentMethod = purchaseProductInfo.PaymentMethod,
                        TransactionStatus = PaymentStatus.completed,
                        PurchasePaymentId = purchasePaymentInformationSuccess.Id,
                        ProductId = purchaseProductInfo.ProductId,
                    };
                    transactionSuccess= _unitOfWork.BuyerRepository.AddPurachaseTransaction(transactionSuccess);


                    PurchaseProductInformation purchaseProductInformation = new PurchaseProductInformation()
                    {
                        ProductId= purchaseProductInfo.ProductId,
                        Title=product.Title,
                        Description=product.Description,
                        Quantity = purchaseProductInfo.Quantity,
                        Price=amount,
                        SellerId= sellerUserId,
                        CreatedAt= DateTime.UtcNow,
                        PurchaseTransactionID=transactionSuccess.TID
                    };
                    _unitOfWork.BuyerRepository.AddPurchaseProductInformation(purchaseProductInformation);
                    GeneralResponse<PurchaseProductInformation> res = new GeneralResponse<PurchaseProductInformation>()
                    {
                        StatusCode = 200,
                        ResponseData = purchaseProductInformation,
                        ResponseMessage="Purchase Successful"
                    };

                    //Sending email
                    ProductEmailBodyDTO productEmailBodyDTO = new ProductEmailBodyDTO()
                    {
                        ProductName=product.Title,
                        Amount=amount,
                        Quantity=purchaseProductInfo.Quantity
                    };
                    _emailService.SendEmailAsync(user.Email, "Your Order is Successful", JsonSerializer.Serialize(productEmailBodyDTO));

                    return res;
                }
                else
                {
                    PurchasePaymentInformation purchasePaymentInformationFailed = new PurchasePaymentInformation()
                    {
                        OriginalTransactionId = originalTransactionId,
                        PaymentMethod = purchaseProductInfo.PaymentMethod,
                        Status = PaymentStatus.failure
                    };

                    purchasePaymentInformationFailed = _unitOfWork.BuyerRepository.AddPurchasePaymentInformation(purchasePaymentInformationFailed);
                    PurchaseTransaction transactionFailed = new PurchaseTransaction()
                    {
                        OriginalTransactionId = originalTransactionId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Amount = amount,
                        PurchasedBy = userId,
                        TransactionType = TransactionType.purchase,
                        PaymentMethod = purchaseProductInfo.PaymentMethod,
                        TransactionStatus = PaymentStatus.failure,
                        PurchasePaymentId = purchasePaymentInformationFailed.Id,
                        ProductId = purchaseProductInfo.ProductId,
                    };
                    _unitOfWork.BuyerRepository.AddPurachaseTransaction(transactionFailed);
                    GeneralResponse<PurchaseProductInformation> res = new GeneralResponse<PurchaseProductInformation>()
                    {
                        StatusCode = 200,
                        ResponseData = null,
                        ResponseMessage = "Payment Failed",
                    };
                    _emailService.SendEmailAsync(user.Email, "Your Order is Unsuccessful", "Payment Failed");
                    return res;
                }
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            
        }
        /// <summary>
        /// Dummy method for gateway response
        /// </summary>
        /// <returns>Payment success or failed</returns>
        private bool IsPaymentSuccess()
        {
            Random random = new Random();

            return random.Next(2) == 0;
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
        IEnumerable<BuyerProduct> IBuyerService.GetProducts(string search, string category, int? page, int pageSize,string sortBy,string sortOrder)
        {
            try
            {
                IEnumerable<Product> buyerProducts = _unitOfWork.BuyerRepository.GetQueryableProducts(search, category, page, pageSize, sortBy, sortOrder);
                List<BuyerProduct> products = new List<BuyerProduct>();
                foreach (Product product in buyerProducts)
                {
                    BuyerProduct buyerProduct = new BuyerProduct()
                    {
                        ProductId = product.Id,
                        Title = product.Title,
                        Description = product.Description,
                        Price = product.Price,
                        CategoryName = product.ProductCategory.CategoryDisplayName
                    };
                    products.Add(buyerProduct);
                }
                return products;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            
        }

        /// <summary>
        /// refund transaction
        /// </summary>
        /// <param name="tid">Purchase Transaction Id</param>
        /// <returns>Return <see cref="GeneralResponse{RefundResponseDTO}"/></returns>
        public GeneralResponse<RefundResponseDTO> Refund(Guid tid, Guid userId)
        {
            
            PurchaseTransaction purchaseTransaction = _unitOfWork.BuyerRepository.GetPurchaseTransaction(tid);

            if(purchaseTransaction == null)
            {
                GeneralResponse<RefundResponseDTO> response = new GeneralResponse<RefundResponseDTO>()
                {
                    StatusCode = 400,
                    ResponseData = null,
                    ResponseMessage = "Error Encountered",
                    ErrorMessage = "Invalid TransactionId"
                };
                return response;
            }

            RefundValidationResponseDTO isRefundable = _unitOfWork.BuyerRepository.IsValidRefundable(tid, userId);

            if(!isRefundable.Status)
            {
                GeneralResponse<RefundResponseDTO> response = new GeneralResponse<RefundResponseDTO>()
                {
                    StatusCode = 400,
                    ResponseData = null,
                    ResponseMessage = null,
                    ErrorMessage = isRefundable.Message
                };
                return response;
            }

            PurchasePaymentInformation purchasePaymentInformation = new PurchasePaymentInformation()
            {
                OriginalTransactionId = purchaseTransaction.OriginalTransactionId,
                PaymentMethod = purchaseTransaction.PaymentMethod,
                Status = PaymentStatus.pending
            };

            purchasePaymentInformation = _unitOfWork.BuyerRepository.AddPurchasePaymentInformation(purchasePaymentInformation);
            PurchaseTransaction transaction = new PurchaseTransaction()
            {
                OriginalTransactionId = purchaseTransaction.OriginalTransactionId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Amount = purchaseTransaction.Amount,
                PurchasedBy = purchaseTransaction.PurchasedBy,
                TransactionType = TransactionType.refund,
                PaymentMethod = purchaseTransaction.PaymentMethod,
                TransactionStatus = PaymentStatus.pending,
                PurchasePaymentId = purchasePaymentInformation.Id,
                ProductId = purchaseTransaction.ProductId,
            };
            _unitOfWork.BuyerRepository.AddPurachaseTransaction(transaction);

            User user = _unitOfWork.AuthRepository.GetUserById(purchaseTransaction.PurchasedBy);

            Product product = _unitOfWork.BuyerRepository.GetProductById(purchaseTransaction.ProductId);
            int quantity = Convert.ToInt32(purchaseTransaction.Amount / product.SellingPrice);



            bool isSuccess = IsPaymentSuccess();

            if (isSuccess)
            {
                PurchasePaymentInformation purchasePaymentInformationSuccess = new PurchasePaymentInformation()
                {
                    OriginalTransactionId = purchaseTransaction.OriginalTransactionId,
                    PaymentMethod = purchaseTransaction.PaymentMethod,
                    Status = PaymentStatus.completed
                };
                purchasePaymentInformationSuccess = _unitOfWork.BuyerRepository.AddPurchasePaymentInformation(purchasePaymentInformationSuccess);
                    
                    
                if (purchaseTransaction.PaymentMethod.Equals(PaymentMethod.wallet) || purchaseTransaction.PaymentMethod.Equals(PaymentMethod.cash) )
                {
                    decimal updatedBalance = user.WalletBalance + purchaseTransaction.Amount;
                    _unitOfWork.AuthRepository.ChangeWalletBalance(user.UserId, updatedBalance);
                }

                Guid sellerUserId = product.CreatedBy;
                User seller = _unitOfWork.AuthRepository.GetUserById(sellerUserId);
                decimal sellerRemainingAmount = seller.WalletBalance - purchaseTransaction.Amount;
                _unitOfWork.AuthRepository.ChangeWalletBalance(sellerUserId, sellerRemainingAmount);
                int remainingStockCount = product.StockCount + quantity;
                _unitOfWork.BuyerRepository.ChangeStockQuantity(product, remainingStockCount);

                PurchaseTransaction transactionSuccess = new PurchaseTransaction()
                {
                    OriginalTransactionId = purchaseTransaction.OriginalTransactionId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Amount = purchaseTransaction.Amount,
                    PurchasedBy = purchaseTransaction.PurchasedBy,
                    TransactionType = TransactionType.refund,
                    PaymentMethod = purchaseTransaction.PaymentMethod,
                    TransactionStatus = PaymentStatus.completed,
                    PurchasePaymentId = purchasePaymentInformationSuccess.Id,
                    ProductId = purchaseTransaction.ProductId,
                };
                transactionSuccess = _unitOfWork.BuyerRepository.AddPurachaseTransaction(transactionSuccess);

                //Email Send to user for successful refund
                EmailData emailData = new EmailData()
                {
                    ProductName = product.Title,
                    Quantity = quantity,
                    Price = purchaseTransaction.Amount
                };
                _emailService.SendEmailAsync(user.Email, "Your Order Successfully Refunded", JsonSerializer.Serialize(emailData));

                GeneralResponse<RefundResponseDTO> res = new GeneralResponse<RefundResponseDTO>()
                {
                    StatusCode = 200,
                    ResponseData = null,
                    ResponseMessage = "Refund Successful"
                };

                return res;
            }
            else
            {
                PurchasePaymentInformation purchasePaymentInformationFailed = new PurchasePaymentInformation()
                {
                    OriginalTransactionId = purchaseTransaction.OriginalTransactionId,
                    PaymentMethod = purchaseTransaction.PaymentMethod,
                    Status = PaymentStatus.failure
                };
                purchasePaymentInformationFailed = _unitOfWork.BuyerRepository.AddPurchasePaymentInformation(purchasePaymentInformationFailed);
                    
                PurchaseTransaction transactionFailed = new PurchaseTransaction()
                {
                    OriginalTransactionId = purchaseTransaction.OriginalTransactionId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Amount = purchaseTransaction.Amount,
                    PurchasedBy = purchaseTransaction.PurchasedBy,
                    TransactionType = TransactionType.refund,
                    PaymentMethod = purchaseTransaction.PaymentMethod,
                    TransactionStatus = PaymentStatus.failure,
                    PurchasePaymentId = purchasePaymentInformationFailed.Id,
                    ProductId = purchaseTransaction.ProductId,

                };
                _unitOfWork.BuyerRepository.AddPurachaseTransaction(transactionFailed);

                EmailData emailData = new EmailData()
                {
                    ProductName = product.Title,
                    Quantity = quantity,
                    Price = purchaseTransaction.Amount
                };
                _emailService.SendEmailAsync(user.Email, "Your Order Refund Failed", JsonSerializer.Serialize(emailData));

                GeneralResponse<RefundResponseDTO> res = new GeneralResponse<RefundResponseDTO>()
                {
                    StatusCode = 500,
                    ResponseData = null,
                    ResponseMessage = "Refund Failed",
                };
                return res;
            }
        }
    } 
}
