using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using StoreManagement.API.Controllers;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.API.Controllers
{
    public class StoresController : BaseApiController, IStoreService
    {
        // GET api/Stores
        public Store GetStores(String domainName)
        {
            return this.StoreRepository.GetStoreByDomain(domainName);
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