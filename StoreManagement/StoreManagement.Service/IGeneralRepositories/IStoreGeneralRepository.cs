using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.IGeneralRepositories
{
    public interface IStoreGeneralRepository : IGeneralRepository
    {
        Store GetStoreByDomain(string domainName);
        Store GetStore(String domain);
        Store GetStore(int id);
        Store GetStoreByUserName(String userName);
        Boolean GetStoreCacheStatus(int id);
        int GetStoreIdByDomain(string domainName);
        Task<Store> GetStoreIdByDomainAsync(string domainName);
        Task<Store> GetStoreAsync(int storeId);
    }
}
