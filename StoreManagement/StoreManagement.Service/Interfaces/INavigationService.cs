using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.Interfaces
{
    public interface INavigationService : IService
    {
        List<Navigation> GetStoreNavigations(int storeId);
        List<Navigation> GetStoreActiveNavigations(int storeId);
    }
}
