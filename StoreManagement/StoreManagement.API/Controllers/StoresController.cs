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

namespace MvcAdminTemplate.Controllers
{
    public class StoresController : BaseApiController
    {
        // GET api/Stores
        public Store GetStores(String domainName)
        {
            return this.StoreRepository.GetStoreByDomain(domainName);
        }
       
    }
}