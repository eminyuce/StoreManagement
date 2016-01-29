using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.IGeneralRepositories
{
    public interface IRetailerGeneralRepository : IGeneralRepository
    {
        Task<List<Retailer>> GetRetailersAsync(int storeId, int ? take, bool isActive);
        Task<Retailer> GetRetailerAsync(int retailerId);
        List<Retailer> GetRetailers(int storeId, int? take, bool isActive);
    }
}
