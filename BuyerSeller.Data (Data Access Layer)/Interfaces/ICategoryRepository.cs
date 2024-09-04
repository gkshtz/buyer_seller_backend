using BuyerSeller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuyerSeller;
using BuyerSeller.Data__Data_Access_Layer_.DTOs;
using System.Security.Cryptography.X509Certificates;


namespace BuyerSeller.Data__Data_Access_Layer_.Interfaces
{
    public interface ICategoryRepository
    {
        /// <summary>
        /// get list of category
        /// </summary>
        /// <returns>List of product category</returns>
        public Task<List<ProductCategoryDTO>> CategoryListAsync();

        /// <summary>
        /// Create new product category
        /// </summary>
        /// <param name="productcategoryDetails">Details for creating product category</param>
        /// <returns>Return  new product category details</returns>
        public Task<ProductCategoryDTO> CreateCategoryAsync(ProductCategoryDTO productcategoryDetails);

        /// <summary>
        /// Update product category
        /// </summary>
        /// <param name="productcategoryDetails">Updated Details</param>
        /// <param name="id">Id of Product category</param>
        /// <returns>Updated Product category details</returns>
        public Task<ProductCategoryDTO> UpdateCategoryAsync(UpdateProductCategoryDTO productcategoryDetails, int id);

        /// <summary>
        /// Delete Product category
        /// </summary>
        /// <param name="id">Id of Product category</param>
        /// <returns>Return true - successfully deleted</returns>
        public Task<bool> DeleteCategoryAsync(int id);

        /// <summary>
        /// Get product category details
        /// </summary>
        /// <param name="productCategoryDisplayName">category display name</param>
        /// <returns>Return Category Details <see cref="ProductCategoryDTO"/></returns>
        public ProductCategoryDTO GetProductCategoryDetails(string productCategoryDisplayName);

    }
}
