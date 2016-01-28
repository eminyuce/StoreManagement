using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Service.IGeneralRepositories;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface IBrandRepository : IBaseRepository<Brand, int>, IBrandGeneralRepository, IDisposable 
    {

        List<Brand> GetBrandsByStoreId(int storeId, string search);
    }
}
