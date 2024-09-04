using BuyerSeller.Core.Utility;
using BuyerSeller.Data__Data_Access_Layer_.DTOs;
using BuyerSeller.Data__Data_Access_Layer_.Interfaces;
using BuyerSeller.Data__Data_Access_Layer_.UnitOfWork;
using BuyerSeller.Models;
using BuyerSeller.Service__Business_Logic_Layer_.Interfaces;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BuyerSeller.Service__Business_Logic_Layer_.Services
{
    /// <summary>
    /// Seller service
    /// </summary>
    public class SellerService : ISellerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Constructor for seller service
        /// </summary>
        /// <param name="unitOfWork">Unit of work dependency</param>
        public SellerService(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        /// <summary>
        /// Add a new product
        /// </summary>
        /// <param name="userId">User ID of the seller</param>
        /// <param name="product">Product details to be added</param>
        /// <returns>Added product</returns>
        public Product AddProduct(Guid userId, ProductDTO product)
        {
            try
            {
                var addedProduct = _unitOfWork.SellerRepository.AddProduct(userId, product);
                _unitOfWork.save();
                return addedProduct;
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.ToString());
            }
            
        }

        /// <summary>
        /// Seller can delete a product
        /// </summary>
        /// <param name="userId">User ID of the seller</param>
        /// <param name="productId">Id of the product to be deleted</param>
        /// <returns>Result in the form of boolean</returns>
        public bool DeleteProduct(Guid userId, Guid productId)
        {
            try
            {
                var result = _unitOfWork.SellerRepository.DeleteProduct(userId, productId);
                _unitOfWork.save();
                return result;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Get the details of product by ID
        /// </summary>
        /// <param name="userId">ID of the seller</param>
        /// <param name="productId">Id of the product</param>
        /// <returns>Product details</returns>
        public Product GetProduct(Guid userId, Guid productId)
        {
            try
            {
                Product product = _unitOfWork.SellerRepository.GetProduct(userId, productId);
                return product;
            }
            catch( Exception ex) 
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Seller can get the list of products
        /// </summary>
        /// <param name="userId">User ID of the seller</param>
        /// <param name="filter">Fiter by category ID</param>
        /// <param name="search">Search</param>
        /// <param name="size">size per page</param>
        /// <param name="page">page</param>
        /// <returns>List of products</returns>
        public List<Product> GetSellerProducts(Guid userId, string filter, string search, int size, int page)
        {
            try
            {
                List<Product> products = _unitOfWork.SellerRepository.GetSellerProducts(userId, filter, search, size, page);
                return products;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Get start date from which top selling products to be searched
        /// </summary>
        /// <param name="currentDateTime">Current Datetime</param>
        /// <param name="duration">Duration</param>
        /// <param name="number">Number of days, weeks, months</param>
        /// <returns>Start DateTime</returns>
        public DateTime GetStartDateTime(DateTime currentDateTime, string duration, int number)
        {
            const int daysInWeek = 7;
            const int daysInMonth = 30;

            duration = duration.ToLower();

            int numberOfdays = 0;

            switch (duration)
            {
                case "days":
                    numberOfdays = 1;
                    break;
                case "weeks":
                    numberOfdays = daysInWeek * number;
                    break;
                case "months":
                    numberOfdays = daysInMonth * number;
                    break;
            }

            return currentDateTime.AddDays(-numberOfdays);
        }

        /// <summary>
        /// Get the top selling products list
        /// </summary>
        /// <param name="userId">ID of the seller</param>
        /// <param name="duration">Duration like days, weeks, months</param>
        /// <param name="number">Number of days, weeks, months</param>
        /// <returns>List of top selling products</returns>
        public List<TopProductsDTO> TopSellingProducts(Guid userId, string duration, int number)
        {
            try
            {
                DateTime currentDateTime = DateTime.UtcNow;

                DateTime startDateTime = GetStartDateTime(currentDateTime, duration, number);

                List<TopProductsDTO> topProducts = _unitOfWork.SellerRepository.TopSellingProducts(userId, startDateTime);

                return topProducts;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Updates the product
        /// </summary>
        /// <param name="userId">User ID of the seller</param>
        /// <param name="productId">Product ID</param>
        /// <param name="updateProduct">Product details to be updated</param>
        /// <returns>Result in the form of boolean</returns>
        public bool UpdateProduct(Guid userId, Guid productId, ProductDTO updateProduct)
        {
            try
            {
                var result = _unitOfWork.SellerRepository.UpdateProduct(userId, productId, updateProduct);
                _unitOfWork.save();
                return result;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<List<PurchasesDTO>> PurchasesListAsync(Guid userId)
        {
            var result = await _unitOfWork.SellerRepository.PurchasesListAsync(userId);
            return result;
        }

        /// <summary>
        /// Load products data from Csv
        /// </summary>
        /// <param name="csvStream">Csv file-stream</param>
        /// <param name="userInfo">User data from token</param>
        /// <returns>Number of products added</returns>
        public async Task<int> LoadProductsDataFromCsvAsync(Stream csvStream, ClaimsPrincipal userInfo)
        {
            var recipientEmail = userInfo.FindFirstValue("Email");
            var userId = Guid.Parse(userInfo.FindFirstValue("UserId"));

            var productsCount = await _unitOfWork.SellerRepository.LoadProductsFromCsvAsync(userId, csvStream);

            _unitOfWork.save();

            if(recipientEmail != null)
            {
                await _emailService.SendEmailAsync(
                    recipientEmail, 
                    Messages.PRODUCTS_UPLOAD_NOTIFICATION_SUBJECT, 
                    Messages.PRODUCTS_UPLOAD_NOTIFICATION_BODY
                );
            }

            return productsCount;
        }
    }
}
