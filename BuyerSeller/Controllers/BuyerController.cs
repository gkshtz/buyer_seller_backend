using BuyerSeller.Service__Business_Logic_Layer_.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BuyerSeller.Models;
using BuyerSeller.Data__Data_Access_Layer_.DTOs;
using BuyerSeller.Application.DTO;
using BuyerSeller.Application;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using BuyerSeller.Data__Data_Access_Layer_.Interfaces;
namespace BuyerSeller.Application.Controllers
{
    /// <summary>
    /// Buyer controller
    /// </summary>
    [Route("buyer")]
    [ApiController]
    public class BuyerController : ControllerBase
    {
        private readonly ILogger<BuyerController> _logger;
        private readonly IAuthRepository authRepository;
        private readonly IBuyerService _buyerService;

        /// <summary>
        /// Buyer controller
        /// </summary>
        /// <param name="buyerService">Buyer service dependency</param>
        /// <param name="logger">Logger dependency</param>
        public BuyerController(IBuyerService buyerService, ILogger<BuyerController> logger, IAuthRepository authRepository)
        {
            _buyerService = buyerService;
            _logger = logger;
            this.authRepository = authRepository;
        }

        /// <summary>
        /// Give the purchase list for a buyer
        /// </summary>
        /// <param name="transanctionStatusFilter">Transaction status filter</param>
        /// <returns>List of purchases</returns>
        [HttpGet]
        [Route("purchases")]
        [Authorize(Roles ="buyer")]
        public ActionResult PurchaseList([FromQuery] string? transanctionStatusFilter)
        {
            try
            {
                var userId = FetchUserId();

                if (userId == null)
                {
                    return StatusCode(500);
                }

                if (!string.IsNullOrEmpty(transanctionStatusFilter))
                {
                    transanctionStatusFilter = transanctionStatusFilter.ToLower();
                    if (transanctionStatusFilter != "completed" && transanctionStatusFilter != "pending" && transanctionStatusFilter != "failure")
                    {
                        return BadRequest();    
                    }
                }

                List<PurchaseTransaction> purchaseTransactions = _buyerService.GetPurchases(Guid.Parse(userId), transanctionStatusFilter);

                CustomResponse<List<PurchaseTransaction>> response = new CustomResponse<List<PurchaseTransaction>>()
                {
                    StatusCode = 200,
                    ResponseData = purchaseTransactions,
                    ResponseMessage = "Success"
                };

                return Ok(response);
            }
            catch (Exception ex) 
            {
                _logger.LogWarning(LogEvents.ServerError, ex, "Internal Server Error");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Fetch Products
        /// </summary>
        /// <param name="search">Search</param>
        /// <param name="category">Category</param>
        /// <param name="page">Page</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="sortBy">Sort By</param>
        /// <param name="sortOrder">Sort Order</param>
        /// <returns>List of products</returns>
        [HttpGet("products")]
        [Authorize(Roles = "buyer")]
        public ActionResult FetchProducts([FromQuery] string? search, [FromQuery] string? category, [FromQuery] int page=1, [FromQuery] int pageSize = 10, [FromQuery] string sortBy = "title",[FromQuery] string sortOrder = "asc")
        {
            try
            {
                IEnumerable<BuyerProduct> buyerProducts = _buyerService.GetProducts(search, category, page, pageSize,sortBy,sortOrder);


                CustomResponse<IEnumerable<BuyerProduct>> response = new CustomResponse<IEnumerable<BuyerProduct>>()
                {
                    StatusCode = 200,
                    ResponseData = buyerProducts,
                    ResponseMessage = "Success"
                };
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LogEvents.ServerError, ex, "Internal Server Error");
                return StatusCode(500);
            }
        }


        /// <summary>
        /// Api for Purchasing the product
        /// </summary>
        /// <param name="purchaseProductInfo">User Input for quantity,payment method and productId</param>
        /// <returns>Purchased Product Information</returns>
        [HttpPost("purchase-transaction")]
        [Authorize(Roles = "buyer")]
        public IActionResult PurchaseProduct(PurchaseProductInfo purchaseProductInfo)
        {
            try
            {
                var userId = FetchUserId();
                
                GeneralResponse<PurchaseProductInformation> response= _buyerService.PurchaseProduct(Guid.Parse(userId), purchaseProductInfo);
                

                return StatusCode(response.StatusCode, response);
            }
            catch(Exception ex) {
                _logger.LogWarning(LogEvents.ServerError, ex, "Internal Server Error");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Refund Action 
        /// </summary>
        /// <param name="id">Purchase transaction id</param>
        /// <returns><see cref="IActionResult"/></returns>
        [Authorize(Roles = "buyer")]
        [HttpPost]
        [Route("refund/{id}")]
        public IActionResult Refund(Guid id)
        {
            try
            {
                var userId = FetchUserId();

                if (userId == null)
                {
                    return StatusCode(500);
                }

                GeneralResponse<RefundResponseDTO> response =_buyerService.Refund(id, Guid.Parse(userId));
                return StatusCode(response.StatusCode,response);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LogEvents.ServerError, ex, "Internal Server Error");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Extract the user ID of current user
        /// </summary>
        /// <returns>User ID</returns>
        private string? FetchUserId()
        {
            return User.FindFirst("UserId")?.Value;
        }

        /// <summary>
        /// Updating the user's balance
        /// </summary>
        /// <param name="balance"></param>
        /// <returns>Response whether balance has updated or not</returns>
        [HttpPost("changeBalance")]
        [Authorize]
        public IActionResult ChangeBalance(int balance)
        {
            try
            {
                if(balance < 0)
                {
                    CustomResponse<string> res = new CustomResponse<string>()
                    {
                        StatusCode = 400,
                        ResponseData = null,
                        ResponseMessage = "Error",
                        ErrorMessage="Balance can't be negative"
                    };
                    return StatusCode(res.StatusCode, res);
                }
                Guid userId = Guid.Parse(FetchUserId());
                authRepository.ChangeWalletBalance(userId, balance);
                CustomResponse<string> response = new CustomResponse<string>()
                {
                    StatusCode = 200,
                    ResponseData = "Balance Updated Successfully",
                    ResponseMessage = "Success"
                };
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LogEvents.ServerError, ex, "Internal Server Error");
                return StatusCode(500);
            }
        }
    }
}
