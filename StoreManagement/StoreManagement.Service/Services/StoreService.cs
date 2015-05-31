using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Services
{
    public class StoreService : BaseService, IStoreService
    {
        public StoreService(string webServiceAddress) : base(webServiceAddress)
        {

        }

        public Store GetStoreByDomain(string domainName)
        {
            throw new NotImplementedException();
        }

        public Store GetStore(string domain)
        {
            throw new NotImplementedException();
        }


        public Store GetSingle(int id)
        {
            throw new NotImplementedException();
        }
    }
}
