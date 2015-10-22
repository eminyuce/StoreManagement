using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface IRetailerRepository : IBaseRepository<Retailer, int>, IRetailerService, IDisposable 
    {
        List<Retailer> GetRetailersByStoreId(int storeId, string search);
    }
}
