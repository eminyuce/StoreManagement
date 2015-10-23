using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface IStorePageDesignRepository : IBaseRepository<StorePageDesign, int>, IDisposable 
    {
        List<StorePageDesign> GetStorePageDesignsByStoreId(string search);
        List<StorePageDesign> GetActiveStoreDesings();
    }
}
