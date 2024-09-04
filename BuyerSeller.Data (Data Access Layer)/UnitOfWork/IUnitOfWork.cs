using BuyerSeller.Data__Data_Access_Layer_.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyerSeller.Data__Data_Access_Layer_.UnitOfWork
{
    /// <summary>
    /// Interface for unit of work
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Auth Repository object
        /// </summary>
        public IAuthRepository AuthRepository { get; }

        /// <summary>
        /// Buyers Repository object
        /// </summary>
        public IBuyerRepository BuyerRepository { get; }

        /// <summary>
        /// Seller Repository object
        /// </summary>
        public ISellerRepository SellerRepository { get; }

        /// <summary>
        /// Category Repository Object
        /// </summary>
        public ICategoryRepository CategoryRepository { get; }

        /// <summary>
        /// DbContext class save changes method 
        /// </summary>
        void save();
    }
}
