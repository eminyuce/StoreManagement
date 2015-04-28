using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Service.Repositories
{
    public class ProductRepository : EntityRepository<Product, int>, IProductRepository
    {
        private IStoreContext dbContext;
        public ProductRepository(IStoreContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
