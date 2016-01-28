using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.Paging;
using StoreManagement.Service.IGeneralRepositories;

namespace StoreManagement.Service.ApiRepositories
{
    public class ProductCategoryApiRepository : BaseApiRepository, IProductCategoryGeneralRepository
    {

        protected override string ApiControllerName { get { return "ProductCategories"; } }


        public ProductCategoryApiRepository(string webServiceAddress)
            : base(webServiceAddress)
        {
        }

        public List<ProductCategory> GetProductCategoriesByStoreId(int storeId)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetProductCategoriesByStoreId?storeId={2}", WebServiceAddress, ApiControllerName, storeId);
            return HttpRequestHelper.GetUrlResults<ProductCategory>(url);
        }

        public List<ProductCategory> GetProductCategoriesByStoreIdWithContent(int storeId)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetProductCategoriesByStoreIdWithContent?storeId={2}", WebServiceAddress, ApiControllerName, storeId);
            return HttpRequestHelper.GetUrlResults<ProductCategory>(url);
        }

        public List<ProductCategory> GetProductCategoriesByStoreId(int storeId, string type)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetProductCategoriesByStoreId?storeId={2}&type={3}", WebServiceAddress, ApiControllerName, storeId, type);
            return HttpRequestHelper.GetUrlResults<ProductCategory>(url);
        }

        public List<ProductCategory> GetProductCategoriesByStoreId(int storeId, string type, string search)
        {
            throw new NotImplementedException();
        }

        public List<ProductCategory> GetProductCategoriesByStoreIdFromCache(int storeId, string type)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetProductCategoriesByStoreIdFromCache?storeId={2}&type={3}", WebServiceAddress, ApiControllerName, storeId, type);
            return HttpRequestHelper.GetUrlResults<ProductCategory>(url);
        }

        public ProductCategory GetProductCategory(int id)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetProductCategory?id={2}", WebServiceAddress, ApiControllerName, id);
            return HttpRequestHelper.GetUrlResult<ProductCategory>(url);
        }

        public StorePagedList<ProductCategory> GetProductCategoryWithContents(int categoryId, int page, int pageSize = 25)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetProductCategoryWithContents?categoryId={2}&page={3}&pageSize={4}", WebServiceAddress, ApiControllerName, categoryId, page, pageSize);
            return HttpRequestHelper.GetUrlPagedResults<ProductCategory>(url);
        }

        public Task<List<ProductCategory>> GetProductCategoriesByStoreIdAsync(int storeId, string type, bool? isActive)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetProductCategoriesByStoreIdAsync?storeId={2}&type={3}&isActive={4}", WebServiceAddress, ApiControllerName, storeId, type, isActive);
            return HttpRequestHelper.GetUrlResultsAsync<ProductCategory>(url);
        }

        public Task<StorePagedList<ProductCategory>> GetProductCategoriesByStoreIdAsync(int storeId, string type, bool? isActive, int page, int pageSize = 25)
        {
            try
            {
                SetCache();
                string url = string.Format("http://{0}/api/{1}/GetProductCategoriesByStoreIdAsync?storeId={2}&type={3}&isActive={4}&page={5}&pageSize={6}",
                    WebServiceAddress, ApiControllerName, storeId, type, isActive, page, pageSize);
                return HttpRequestHelper.GetUrlPagedResultsAsync<ProductCategory>(url);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                return null;
            }
        }

        public Task<ProductCategory> GetProductCategoryAsync(int storeId, int productId)
        {
            try
            {
                SetCache();
                string url = string.Format("http://{0}/api/{1}/GetProductCategoryAsync?storeId={2}&productId={3}", WebServiceAddress, ApiControllerName, storeId, productId);
                return HttpRequestHelper.GetUrlResultAsync<ProductCategory>(url);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                return null;
            }
        }

        public Task<ProductCategory> GetProductCategoryAsync(int categoryId)
        {
            try
            {
                SetCache();
                string url = string.Format("http://{0}/api/{1}/GetProductCategoryAsync?categoryId={2}", WebServiceAddress, ApiControllerName, categoryId);
                return HttpRequestHelper.GetUrlResultAsync<ProductCategory>(url);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                return null;
            }
        }

        public Task<List<ProductCategory>> GetCategoriesByBrandIdAsync(int storeId, int brandId)
        {
            try
            {
                SetCache();
                string url = string.Format("http://{0}/api/{1}/GetCategoriesByBrandIdAsync?storeId={2}&brandId={3}", WebServiceAddress, ApiControllerName, storeId, brandId);
                return HttpRequestHelper.GetUrlResultsAsync<ProductCategory>(url);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                return null;
            }
        }

        public Task<List<ProductCategory>> GetCategoriesByRetailerIdAsync(int storeId, int retailerId)
        {
            try
            {
                SetCache();
                string url = string.Format("http://{0}/api/{1}/GetCategoriesByRetailerIdAsync?storeId={2}&retailerId={3}", WebServiceAddress, ApiControllerName, storeId, retailerId);
                return HttpRequestHelper.GetUrlResultsAsync<ProductCategory>(url);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                return null;
            }
        }


        protected override void SetCache()
        {
            HttpRequestHelper.CacheMinute = CacheMinute;
            HttpRequestHelper.IsCacheEnable = IsCacheEnable;
        }
    }
}
