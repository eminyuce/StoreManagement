using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.Interfaces
{
    public interface IStoreService : IService
    {
        Store GetStoreByDomain(string domainName);
        Store GetStore(String domain);
        Store GetStore(int id);
        Store GetStoreByUserName(String userName);
        Boolean GetStoreCacheStatus(int id);
        int GetStoreIdByDomain(string domainName);
    }
}
