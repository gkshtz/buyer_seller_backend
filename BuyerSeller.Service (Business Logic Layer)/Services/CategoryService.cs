using BuyerSeller.Core.Middlewares.Exceptions;
using BuyerSeller.Data__Data_Access_Layer_.DTOs;
using BuyerSeller.Data__Data_Access_Layer_.Interfaces;
using BuyerSeller.Data__Data_Access_Layer_.UnitOfWork;
using BuyerSeller.Service__Business_Logic_Layer_.Interfaces;
using Microsoft.AspNetCore.Http;
using System;


namespace BuyerSeller.Service__Business_Logic_Layer_.Services
{
    /// <summary>
    /// Services for category
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Instantiate Category Service with category repository
        /// </summary>
        /// <param name="unitOfWork">Unit of work dependency</param>
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get List of category
        /// </summary>
        /// <returns>List of category</returns>
        public async Task<List<ProductCategoryDTO>> CategoryListAsync()
        {
            List<ProductCategoryDTO> productCategories = await _unitOfWork.CategoryRepository.CategoryListAsync();
            return productCategories;
        }

        /// <summary>
        /// Create new category
        /// </summary>
        /// <param name="productCategoryDetails">Data for creating catgeory</param>
        /// <returns>Details of created category</returns>
        public async Task<ProductCategoryDTO> CreateCategoryAsync(ProductCategoryDTO productCategoryDetails)
        { 

            if(productCategoryDetails.CategoryDisplayName == null)
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Enter product category details");
            }

            ProductCategoryDTO newProductCategory = await _unitOfWork.CategoryRepository.CreateCategoryAsync(productCategoryDetails);

            if(newProductCategory == null)
            {
                return null;
            }

            _unitOfWork.save();
            ProductCategoryDTO responseNewProductCategory = _unitOfWork.CategoryRepository.GetProductCategoryDetails(newProductCategory.CategoryDisplayName);
            return responseNewProductCategory;
        }

        /// <summary>
        /// Delete Existing Category
        /// </summary>
        /// <param name="id">CategoryId</param>
        /// <returns>Returns True if category deleted successfully</returns>
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            bool deleteCategory = await _unitOfWork.CategoryRepository.DeleteCategoryAsync(id);

            if (deleteCategory)
            {
                _unitOfWork.save();
            }

            return deleteCategory;
        }

        /// <summary>
        /// Update existing category
        /// </summary>
        /// <param name="productCategoryDetails">data for updating category</param>
        /// <param name="id">categoryId</param>
        /// <returns>Details of updated category</returns>
        public async Task<ProductCategoryDTO> UpdateCategoryAsync(UpdateProductCategoryDTO productCategoryDetails, int id)
        {
            if(productCategoryDetails == null)
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Please provide category details");
            }

            ProductCategoryDTO updatedProductCategory = await _unitOfWork.CategoryRepository.UpdateCategoryAsync(productCategoryDetails, id);
            _unitOfWork.save();

            return updatedProductCategory;
        }
    }
}
