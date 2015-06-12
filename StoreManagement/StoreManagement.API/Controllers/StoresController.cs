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
    public class StoresController : BaseApiController<Store>, IStoreService
    {
        // GET api/Stores
        public Store GetStores(String domainName)
        {
            return this.StoreRepository.GetStoreByDomain(domainName);
        }

        public Store GetStoreByDomain(string domainName)
        {
            return this.StoreRepository.GetStoreByDomain(domainName);
        }

        public Store GetStore(string domain)
        {
            return this.StoreRepository.GetStore(domain);
        }

        public Store GetStore(int id)
        {
            return this.StoreRepository.GetSingle(id);
        }

        public Store GetStoreByUserName(string userName)
        {
            return this.StoreRepository.GetStoreByUserName(userName);
        }

        public override IEnumerable<Store> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Store Get(int id)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Post(Store value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Put(int id, Store value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}