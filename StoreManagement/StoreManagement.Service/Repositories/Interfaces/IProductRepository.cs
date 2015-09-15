using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface IProductRepository : IBaseRepository<Product, int>, IProductService, IDisposable 
    {
        List<Product> GetProductsByStoreId(int storeId, String searchKey);
        List<Product> GetMainPageProducts(int storeId, int? take);
    }
}
