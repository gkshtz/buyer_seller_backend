using AutoMapper;
using BuyerSeller.Core.Middlewares.Exceptions;
using BuyerSeller.Data__Data_Access_Layer_.Data;
using BuyerSeller.Data__Data_Access_Layer_.DTOs;
using BuyerSeller.Data__Data_Access_Layer_.Interfaces;
using BuyerSeller.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BuyerSeller.Data__Data_Access_Layer_.Repositories
{
    /// <summary>
    /// Repository For Category
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        private AppDBcontext _dbContext;

        /// <summary>
        /// Initializes instance of category repository with database context and dto mapper
        /// </summary>
        /// <param name="dbContext">Instance of database context</param>
        /// <param name="mapper">Instance of auto mapper</param>
        public CategoryRepository(AppDBcontext dbContext)
        {
            _dbContext = dbContext; 
        }

        /// <summary>
        /// Fetch List of category from database Context
        /// </summary>
        /// <returns>List of category</returns>
        public async Task<List<ProductCategoryDTO>> CategoryListAsync()
        {
            var productCategoriesList = await _dbContext.ProductCategories
                .Select(pc => new ProductCategoryDTO
                {
                    CategoryId = pc.CategoryId,
                    CategoryDisplayName = pc.CategoryDisplayName,
                    IsActive = pc.IsActive
                })
                .ToListAsync();
            return productCategoriesList;
        }

        /// <summary>
        /// Create new category
        /// </summary>
        /// <param name="productcategoryDetails">data for new category</param>
        /// <returns>Details of new category</returns>
        public async Task<ProductCategoryDTO> CreateCategoryAsync(ProductCategoryDTO productCategoryDetails)
        {
            bool categoryExist = _dbContext.ProductCategories
                                           .Any(pc => pc.CategoryDisplayName.ToLower() == productCategoryDetails.CategoryDisplayName.ToLower());

            if (categoryExist)
            {
                return null;
            }

            // Convert ProductCategoryDTO to ProductCategoryEntity
            ProductCategory newProductCategory = new ProductCategory
            {
                CategoryDisplayName = productCategoryDetails.CategoryDisplayName,
                IsActive = (bool)productCategoryDetails.IsActive
            };
            await _dbContext.ProductCategories.AddAsync(newProductCategory);

            return productCategoryDetails;

        }

        /// <summary>
        /// Updating existing category
        /// </summary>
        /// <param name="productCategoryDetails">Data for updating existing category</param>
        /// <param name="id">Id of category</param>
        /// <returns>Details of updated category</returns>
        /// <exception cref="Exception"></exception>
        public async Task<ProductCategoryDTO> UpdateCategoryAsync(UpdateProductCategoryDTO productCategoryDetails, int id)
        {
            ProductCategory productCategoryObject = await _dbContext.ProductCategories.FirstOrDefaultAsync(
                pc => pc.CategoryId == id
            );

            if(productCategoryObject == null)
            {
                throw new CustomException(StatusCodes.Status500InternalServerError, "Invalid category id");
            }

            if (productCategoryDetails.CategoryDisplayName != null)
            {
                productCategoryObject.CategoryDisplayName = productCategoryDetails.CategoryDisplayName;
            }

            if(productCategoryDetails.IsActive != null)
            {
                productCategoryObject.IsActive = (bool)productCategoryDetails.IsActive;
            }
            
            productCategoryObject.CreatedAt = DateTime.UtcNow;

            ProductCategoryDTO updatedProductCategory = new ProductCategoryDTO
            {
                CategoryId = productCategoryObject.CategoryId,
                CategoryDisplayName = productCategoryObject.CategoryDisplayName,
                IsActive = (bool)productCategoryObject.IsActive
            };


            return updatedProductCategory;
        }

        /// <summary>
        /// Delete existing category
        /// </summary>
        /// <param name="id">Id of existing category</param>
        /// <returns>Return true if succesfully deleted</returns>
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            ProductCategory? productCategoryToDelete = await _dbContext.ProductCategories
                .FirstOrDefaultAsync(pc => pc.CategoryId == id);

            if (productCategoryToDelete == null)
            {
                throw new CustomException(StatusCodes.Status500InternalServerError, "Invalid category id");
            }

            int countProductInCategory = await _dbContext.Products.CountAsync(p => p.CategoryId == productCategoryToDelete.CategoryId);
            
            if (countProductInCategory > 0)
            {
                return false;
            }
            _dbContext.ProductCategories.Remove(productCategoryToDelete);

            return true;
        }

        public ProductCategoryDTO GetProductCategoryDetails(string productCategoryDisplayName)
        {
            ProductCategory productCategory = _dbContext.ProductCategories.FirstOrDefault(pc => pc.CategoryDisplayName.ToLower() == productCategoryDisplayName.ToLower());
            ProductCategoryDTO responseProductCategory = new ProductCategoryDTO()
            {
                CategoryId = productCategory.CategoryId,
                CategoryDisplayName = productCategory.CategoryDisplayName,
                IsActive = productCategory.IsActive
            };

            return responseProductCategory;
        }


    }
}
