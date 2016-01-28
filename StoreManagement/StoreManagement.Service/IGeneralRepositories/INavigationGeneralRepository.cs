using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.IGeneralRepositories
{
    public interface INavigationGeneralRepository : IGeneralRepository
    {
        List<Navigation> GetStoreNavigations(int storeId);
        List<Navigation> GetStoreActiveNavigations(int storeId);
        Task<List<Navigation>> GetStoreActiveNavigationsAsync(int storeId);
    }

}
