using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using StoreManagement.Data.Entities;
using StoreManagement.Service.IGeneralRepositories;

namespace StoreManagement.API.Controllers
{
    public class RetailersController : BaseApiController<Retailer>, IRetailerGeneralRepository
    {
        public override IEnumerable<Retailer> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Retailer Get(int id)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Post(Retailer value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Put(int id, Retailer value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Retailer>> GetRetailersAsync(int storeId, int? take, bool isActive)
        {
            return await RetailerRepository.GetRetailersAsync(storeId, take, isActive);
        }

        public async Task<Retailer> GetRetailerAsync(int retailerId)
        {
            return await RetailerRepository.GetRetailerAsync(retailerId);
        }

        public List<Retailer> GetRetailers(int storeId, int? take, bool isActive)
        {
            return RetailerRepository.GetRetailers(storeId, take, isActive);
        }
    }
}
