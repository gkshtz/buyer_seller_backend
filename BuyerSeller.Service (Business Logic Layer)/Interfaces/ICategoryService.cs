using BuyerSeller.Models;
using BuyerSeller.Data__Data_Access_Layer_.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyerSeller.Service__Business_Logic_Layer_.Interfaces
{
    public interface ICategoryService
    {
        /// <summary>
        /// Get Product Category List
        /// </summary>
        /// <returns>List of ProductCategory</returns>
        public Task<List<ProductCategoryDTO>> CategoryListAsync();

        /// <summary>
        /// Create New Product Category
        /// </summary>
        /// <param name="productCategoryDetails">Data for creating a Product Category</param>
        /// <returns>Details of New Product Category</returns>
        public Task<ProductCategoryDTO> CreateCategoryAsync(ProductCategoryDTO productCategoryDetails);

        /// <summary>
        /// Updating the existing category
        /// </summary>
        /// <param name="productCategoryDetails">Data of updated category</param>
        /// <param name="id">ProductCategoryId</param>
        /// <returns>Details of Updated Category</returns>
        public Task<ProductCategoryDTO> UpdateCategoryAsync(UpdateProductCategoryDTO productCategoryDetails, int id);

        /// <summary>
        /// Delete Category
        /// </summary>
        /// <param name="id">ProductCategoryId</param>
        /// <returns>Returns True if category deleted successfully </returns>
        public Task<bool> DeleteCategoryAsync(int id);

        

    }
}
