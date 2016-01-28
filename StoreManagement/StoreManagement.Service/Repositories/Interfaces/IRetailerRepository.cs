using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.IGeneralRepositories;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface IRetailerRepository : IBaseRepository<Retailer, int>, IRetailerGeneralRepository, IDisposable 
    {
        List<Retailer> GetRetailersByStoreId(int storeId, string search);
    }
}
