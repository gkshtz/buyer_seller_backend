using AutoMapper;
using BuyerSeller.Data__Data_Access_Layer_.Data;
using BuyerSeller.Data__Data_Access_Layer_.Interfaces;
using BuyerSeller.Data__Data_Access_Layer_.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyerSeller.Data__Data_Access_Layer_.UnitOfWork
{
    /// <summary>
    /// Unit of work class
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBcontext _dbcontext;
        private readonly IMapper _mapper;

        private IBuyerRepository _buyerRepository;
        private ISellerRepository _sellerRepository;
        private IAuthRepository _authRepository;
        private ICategoryRepository _categoryRepository;

        /// <summary>
        /// Unit of work constructor
        /// </summary>
        /// <param name="_dbcontext">AppDBcontext dependency</param>
        public UnitOfWork(AppDBcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        /// <summary>
        /// Auth Repository
        /// </summary>
        public IAuthRepository AuthRepository
        {
            get { return _authRepository ?? (_authRepository = new AuthRepository(_dbcontext)); }
        }

        /// <summary>
        /// Buyer Repository
        /// </summary>
        public IBuyerRepository BuyerRepository
        {
            get { return _buyerRepository ?? (_buyerRepository = new BuyerRepository(_dbcontext)); }
        }

        /// <summary>
        /// Seller Repository
        /// </summary>
        public ISellerRepository SellerRepository
        {
            get { return _sellerRepository ?? (_sellerRepository = new SellerRepository(_dbcontext)); }
        }

        /// <summary>
        /// Category Repository
        /// </summary>
        public ICategoryRepository CategoryRepository
        {
            get { return _categoryRepository ?? (_categoryRepository = new CategoryRepository(_dbcontext)); }
        }

        /// <summary>
        /// Dispose dbcontext method
        /// </summary>
        public void Dispose()
        {
            _dbcontext.Dispose();
        }

        /// <summary>
        /// DBcontect save changes method
        /// </summary>
        public void save()
        {
            _dbcontext.SaveChanges();
        }
    }
}
