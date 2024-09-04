using BuyerSeller.Application.DTO;
using BuyerSeller.Application.Validations;
using BuyerSeller.Data__Data_Access_Layer_.DTOs;
using BuyerSeller.Models;
using BuyerSeller.Service__Business_Logic_Layer_.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;

namespace BuyerSeller.Application.Controllers
{
    /// <summary>
    /// Seller controller
    /// </summary>
    [Route("seller")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly ISellerService _sellerService;
        private readonly ILogger<SellerController> _logger;
        public SellerController(ISellerService sellerService, ILogger<SellerController> logger)
        {
            _sellerService = sellerService;
            _logger = logger;
        }

        /// <summary>
        /// Seller controller constructor
        /// </summary>
        /// <param name="filter">filter</param>
        /// <param name="search">search</param>
        /// <param name="size">size</param>
        /// <param name="page">page</param>
        /// <returns></returns>
        [Route("products")]
        [HttpGet]
        [Authorize(Roles = "seller")]
        public ActionResult ProductsList([FromQuery] string filter, [FromQuery] string search, [FromQuery] int size = 10, [FromQuery] int page = 1)
        {
            try
            {
                var userId = FetchUserId();

                if (userId == null) 
                {
                    return StatusCode(500);
                }

                List<Product> products = _sellerService.GetSellerProducts(Guid.Parse(userId), filter, search, size, page);

                CustomResponse<List<Product>> response = new CustomResponse<List<Product>>()
                {
                    StatusCode = 200,
                    ResponseData = products,
                    ResponseMessage = "Success"
                };

                return Ok(response);
            }
            catch(Exception ex) 
            {
                _logger.LogWarning(LogEvents.ServerError, ex, "Internal Server Error");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Get product by ID
        /// </summary>
        /// <param name="productId">product ID</param>
        /// <returns>Product Details</returns>
        [Route("products/{productId}")]
        [HttpGet]
        [Authorize(Roles = "seller")]
        public ActionResult Get(Guid productId)
        {
            try
            {
                var userId = FetchUserId();

                if (userId == null)
                {
                    return StatusCode(500);
                }

                if (!Guid.TryParse(productId.ToString(), out _))
                {
                    return BadRequest();
                }

                Product product = _sellerService.GetProduct(Guid.Parse(userId), productId);
                if(product == null) 
                {
                    CustomResponse<Product?> errorResponse = new CustomResponse<Product?>()
                    {
                        StatusCode = 404,
                        ResponseData = null,
                        ErrorMessage = "Such product not found"
                    };

                    return NotFound(errorResponse);
                }

                CustomResponse<Product> response = new CustomResponse<Product>()
                {
                    StatusCode = 200,
                    ResponseData = product,
                    ResponseMessage = "Success"
                };

                return Ok(response);
            }
            catch(Exception ex) 
            {
                _logger.LogWarning(LogEvents.ServerError, ex, "Internal Server Error");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Add new product
        /// </summary>
        /// <param name="product">product details</param>
        /// <returns>Added product details</returns>
        [Route("products")]
        [HttpPost]
        [Authorize(Roles = "seller")]
        public ActionResult Add([FromBody] ProductDTO product)
        {            
            try
            {
                ProductDTOValidator validator = new ProductDTOValidator();
                var validationResult = validator.Validate(product);
                if (validationResult != null)
                {
                    if (!validationResult.IsValid)
                    {
                        return BadRequest(validationResult.Errors);
                    }
                }

                var userId = FetchUserId();

                if (userId == null)
                {
                    return StatusCode(500);
                }

                if (product == null)
                {
                    return BadRequest();
                }

                var addedProduct = _sellerService.AddProduct(Guid.Parse(userId), product);

                if(addedProduct == null)
                {
                    CustomResponse<Product?> errorResponse = new CustomResponse<Product?>()
                    {
                        StatusCode = 404,
                        ResponseData = null,
                        ErrorMessage = "Failed"
                    };

                    return NotFound(errorResponse);
                }

                CustomResponse<Product> response = new CustomResponse<Product>()
                {
                    StatusCode = 200,
                    ResponseData = addedProduct,
                    ResponseMessage = "Success"
                };

                return Ok(response);
            }
            catch(Exception ex) 
            {
                _logger.LogWarning(LogEvents.ServerError, ex, "Internal Server Error");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Updates the product
        /// </summary>
        /// <param name="productId">Product ID</param>
        /// <param name="updateProduct">Update Product</param>
        /// <returns>Result message</returns>
        [Route("products/{productId}")]
        [HttpPut]
        [Authorize(Roles = "seller")]
        public ActionResult Update(Guid productId, ProductDTO updateProduct) 
        {
            try
            {
                ProductDTOValidator validator = new ProductDTOValidator();
                var validationResult = validator.Validate(updateProduct);
                if (validationResult != null)
                {
                    if (!validationResult.IsValid)
                    {
                        return BadRequest(validationResult.Errors);
                    }
                }

                if (!Guid.TryParse(productId.ToString(), out _))
                {
                    return BadRequest();
                }

                var userId = FetchUserId();

                if (userId == null)
                {
                    return StatusCode(500);
                }

                if (updateProduct == null)
                {
                    return BadRequest();
                }

                var result = _sellerService.UpdateProduct(Guid.Parse(userId), productId, updateProduct);

                if (result == false)
                {
                    CustomResponse<bool> errorResponse = new CustomResponse<bool>()
                    {
                        StatusCode = 404,
                        ResponseData = false,
                        ErrorMessage = "Such product not found"
                    };

                    return NotFound(errorResponse);
                }

                CustomResponse<bool> response = new CustomResponse<bool>()
                {
                    StatusCode = 200,
                    ResponseData = true,
                    ResponseMessage = "Success"
                };

                return Ok(response);
            }
            catch(Exception ex) 
            {
                _logger.LogWarning(LogEvents.ServerError, ex, "Internal Server Error");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Seller can delete a product
        /// </summary>
        /// <param name="productId">product ID</param>
        /// <returns>Result message</returns>
        [HttpDelete]
        [Route("products/{productId}")]
        [Authorize(Roles = "seller")]
        public ActionResult Delete(Guid productId) 
        {
            try
            {
                var userId = FetchUserId();

                if (userId == null)
                {
                    return StatusCode(500);
                }

                if (!Guid.TryParse(productId.ToString(), out _))
                {
                    return BadRequest();
                }

                var result = _sellerService.DeleteProduct(Guid.Parse(userId), productId); 
                
                if(!result)
                {
                    CustomResponse<bool> errorResponse = new CustomResponse<bool>()
                    {
                        StatusCode = 404,
                        ResponseData = false,
                        ErrorMessage = "Such product not found"
                    };

                    return NotFound(errorResponse);
                }

                CustomResponse<bool> response = new CustomResponse<bool>()
                {
                    StatusCode = 200,
                    ResponseData = true,
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
        /// Get a list of top selling products over a period
        /// </summary>
        /// <param name="duration">Duration</param>
        /// <param name="number">Number of days , weeks or months</param>
        /// <returns>List of top selling products</returns>
        [HttpGet]
        [Route("top-products/{duration}")]
        [Authorize(Roles = "seller")]
        public ActionResult TopSellingProducts(string duration, [FromQuery] int number=1) 
        {
            try
            {
                var userId = FetchUserId();

                if (userId == null)
                {
                    return StatusCode(500);
                }

                List<string> allowedDurations = new List<string> {"days", "weeks", "months" };

                if(!allowedDurations.Contains(duration.ToLower()))
                {
                    return BadRequest(new { message = "Invalid duration" });
                }

                if(number <= 0)
                {
                    return BadRequest(new { message = $"Invalid number of {duration}" });
                }

                List<TopProductsDTO> topProducts = _sellerService.TopSellingProducts(Guid.Parse(userId), duration, number);

                CustomResponse<List<TopProductsDTO>> response = new CustomResponse<List<TopProductsDTO>>()
                {
                    StatusCode = 200,
                    ResponseData = topProducts,
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

        [HttpGet]
        [Route("purchases")]
        [Authorize(Roles = "seller")]
        public async Task<ActionResult> PurchasesListAsync()
        {
            var userId = FetchUserId();

            if (userId == null)
            {
                return StatusCode(500);
            }

            List<PurchasesDTO> purchasesList = await _sellerService.PurchasesListAsync(Guid.Parse(userId));
            return Ok(purchasesList);
        }

        /// <summary>
        /// Upload list of products as a csv-file
        /// </summary>
        /// <param name="file">Uploaded file</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/bulk-upload")]
        public async Task<ActionResult> UploadProductsData(IFormFile file)
        {
            var email = User.FindFirst("Email")?.Value;
            int productsCount = await _sellerService.LoadProductsDataFromCsvAsync(file.OpenReadStream(), User);

            return Ok(new CustomResponse<object>
            {
                StatusCode = StatusCodes.Status200OK,
                ResponseMessage = $"{productsCount} Products added successfully!",
                ResponseData = null
            });
        }

        private string? FetchUserId()
        {
            return User.FindFirst("UserId")?.Value;
        }
    }
}
