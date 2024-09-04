using Microsoft.AspNetCore.Mvc;
using BuyerSeller.Service__Business_Logic_Layer_.Interfaces;
using BuyerSeller.Data__Data_Access_Layer_.DTOs;
using Microsoft.AspNetCore.Authorization;
using BuyerSeller.Application.Validations;
using BuyerSeller.Models;
using BuyerSeller.Application.DTO;


namespace BuyerSeller.Application.Controllers
{
    /// <summary>
    /// Product category CRUD
    /// </summary>
    [Route("seller/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private  readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;

        /// <summary>
        /// Initializes Category Controller
        /// </summary>
        /// <param name="categoryService">Instance of <see cref="ICategoryService"/></param>
        /// <param name="logger">Instance of <see cref="ILogger"/></param>
        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;

        }

        /// <summary>
        /// Returns a List of Categories
        /// </summary>
        /// <returns>Success - List&lt;ProductCategoryDTO&gt; <br></br> Failure - Error</returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CategoryList()
        {
            List<ProductCategoryDTO> productCategories = await _categoryService.CategoryListAsync();
            return Ok(productCategories);
        }

        /// <summary>
        /// Create New Category
        /// </summary>
        /// <param name="productCategoryDetails">Data to create Category</param>
        /// <returns>Success - Data of created category <br></br> Failure -  Internal Server Error</returns>
        [Authorize(Roles = "seller")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory(ProductCategoryDTO? productCategoryDetails)
        {
            ProductCategoryDTOValidator validator = new ProductCategoryDTOValidator();
            var validationResult = validator.Validate(productCategoryDetails);
            if (validationResult != null)
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }
            }

            var newProductCategory = await _categoryService.CreateCategoryAsync(productCategoryDetails);

            if(newProductCategory == null)
            {
                return new ConflictObjectResult(new
                {
                    message = "Category already exists."
                });
            }

            return Ok(newProductCategory);
        }

        /// <summary>
        /// Update Existing Category Data
        /// </summary>
        /// <param name="productCategoryDetails">Data of updated category</param>
        /// <param name="id">CategoryId</param>
        /// <returns>Success - Data of updated category <br></br> Failure - Internal Server Error</returns>
        [Authorize(Roles = "seller")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCategoryAsync([FromBody]UpdateProductCategoryDTO productCategoryDetails, int id)
        {
            ProductCategoryDTO updatedProductCategory = await _categoryService.UpdateCategoryAsync(productCategoryDetails, id);
            return Ok(updatedProductCategory);
        }

        /// <summary>
        /// Delete Existing Category
        /// </summary>
        /// <param name="id">Id of category to be deleted</param>
        /// <returns>Success - Ok <br></br> Failure - Internal Server Error</returns>
        [Authorize(Roles = "seller")]
        [HttpDelete("{id}")]
        public async Task<CustomResponse<Object>> DeleteCategoryAsync(int id)
        {
            bool deleteCategory = await _categoryService.DeleteCategoryAsync(id);

            if (!deleteCategory)
            {
                return new CustomResponse<object>
                {
                    StatusCode = 400,
                    ResponseMessage = "Category is not empty!"
                };
            }

            return new CustomResponse<Object>
            {
                StatusCode = 200,
                ResponseMessage = "Category Deleted Successfully!"
            };
        }        
    }
}
