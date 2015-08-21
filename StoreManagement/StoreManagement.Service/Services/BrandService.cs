using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Services
{
    public class BrandService : BaseService, IBrandService
    {
        protected override string ApiControllerName { get { return "Brands"; } }
        public BrandService(string webServiceAddress) : base(webServiceAddress)
        {

        }


        protected override void SetCache()
        {
            HttpRequestHelper.CacheMinute = CacheMinute;
            HttpRequestHelper.IsCacheEnable = IsCacheEnable;
        }

        public Task<List<Brand>> GetBrandsAsync(int storeId, int? take, bool? isActive)
        {
            try
            {
                SetCache();
                string url = string.Format("http://{0}/api/{1}/GetBrandsAsync" +
                                           "?storeId={2}" +
                                           "&take={3}" +
                                           "&isActive={4}", WebServiceAddress, ApiControllerName, storeId, take, isActive);
                return HttpRequestHelper.GetUrlResultsAsync<Brand>(url);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "API:GetBrandsAsync", storeId, take, isActive);
                return null;
            }
        }

        public Task<Brand> GetBrandAsync(int brandId)
        {
            try
            {
                SetCache();
                string url = string.Format("http://{0}/api/{1}/GetBrandAsync" +
                                           "?brandId={2}", WebServiceAddress, ApiControllerName, brandId);
                return HttpRequestHelper.GetUrlResultAsync<Brand>(url);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "API:GetBrandAsync", brandId);
                return null;
            }
        }
    }
}
