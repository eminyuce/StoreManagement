using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;
using WebApi.OutputCache.V2;

namespace StoreManagement.API.Controllers
{
    [CacheOutput(ClientTimeSpan = StoreConstants.CacheClientTimeSpanSeconds, ServerTimeSpan = StoreConstants.CacheServerTimeSpanSeconds)]
    public class BrandsController : BaseApiController<Brand>, IBrandService
    {
        public override IEnumerable<Brand> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Brand Get(int id)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Post(Brand value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Put(int id, Brand value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Brand>> GetBrandsAsync(int storeId, int? take, bool? isActive)
        {
            return await BrandRepository.GetBrandsAsync(storeId, take, isActive);
        }

        public async Task<Brand> GetBrandAsync(int brandId)
        {
            return await BrandRepository.GetBrandAsync(brandId);
        }
    }
}
