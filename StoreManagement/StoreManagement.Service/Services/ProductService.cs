using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.Paging;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Services
{
    public class ProductService : BaseService, IProductService
    {

        protected override string ApiControllerName { get { return "Products"; } }
        public ProductService(string webServiceAddress)
            : base(webServiceAddress)
        {

        }


        public Product GetProductsById(int productId)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetProductsById?productId={2}", WebServiceAddress, ApiControllerName, productId);
            return HttpRequestHelper.GetUrlResult<Product>(url);
        }

        public List<Product> GetProductByType(string typeName)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetProductByType?typeName={2}", WebServiceAddress, ApiControllerName, typeName);
            return HttpRequestHelper.GetUrlResults<Product>(url);
        }

        public List<Product> GetProductByType(int storeId, string typeName)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetProductByType?storeId={2}&typeName={3}", WebServiceAddress, ApiControllerName, storeId, typeName);
            return HttpRequestHelper.GetUrlResults<Product>(url);
        }

        public Product GetProductByUrl(int storeId, string url)
        {
            SetCache();
            string url2 = string.Format("http://{0}/api/{1}/GetProductByUrl?storeId={2}&url={3}", WebServiceAddress, ApiControllerName, storeId, url);
            return HttpRequestHelper.GetUrlResult<Product>(url2);
        }

        public List<Product> GetProductByTypeAndCategoryId(int storeId, string typeName, int categoryId)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetProductByTypeAndCategoryId?" +
                                          "storeId={2}" +
                                          "&typeName={3}&categoryId={4}",
                                          WebServiceAddress,
                                          ApiControllerName,
                                          storeId,
                                          typeName,
                                          categoryId);

            return HttpRequestHelper.GetUrlResults<Product>(url);

        }

        public List<Product> GetProductByTypeAndCategoryId(int storeId, string typeName, int categoryId, string search, bool? isActive)
        {
            try
            {
                SetCache();
                string url = string.Format("http://{0}/api/{1}/GetProductByTypeAndCategoryId" +
                                           "?storeId={2}" +
                                           "&typeName={3}" +
                                           "&categoryId={4}&search={5}", WebServiceAddress, ApiControllerName, storeId, typeName, categoryId, search);
                return HttpRequestHelper.GetUrlResults<Product>(url);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "API:GetProductByTypeAndCategoryId", storeId, typeName, categoryId, search);
                return new List<Product>();
            }
        }

        public List<Product> GetProductByTypeAndCategoryIdFromCache(int storeId, string typeName, int categoryId)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetProductByTypeAndCategoryIdFromCache" +
                                       "?storeId={2}" +
                                       "&typeName={3}" +
                                       "&categoryId={4}", WebServiceAddress, ApiControllerName, storeId, typeName, categoryId);
            return HttpRequestHelper.GetUrlResults<Product>(url);

        }

        public StorePagedList<Product> GetProductsCategoryId(int storeId, int? categoryId, string typeName, bool? isActive, int page, int pageSize)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetProductsCategoryId?storeId={2}" +
                                       "&categoryId={3}" +
                                       "&typeName={4}" +
                                       "&isActive={5}&page={6}&pageSize={7}", WebServiceAddress, ApiControllerName, storeId, categoryId, typeName, isActive, page, pageSize);
            return HttpRequestHelper.GetUrlPagedResults<Product>(url);

        }

        public Product GetProductWithFiles(int id)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetProductWithFiles?id={2}", WebServiceAddress, ApiControllerName, id);
            return HttpRequestHelper.GetUrlResult<Product>(url);

        }

        public Task<StorePagedList<Product>> GetProductsCategoryIdAsync(int storeId, int? categoryId, string typeName, bool? isActive, int page, int pageSize, String search)
        {

            try
            {

                SetCache();
                string url = string.Format("http://{0}/api/{1}/GetProductsCategoryIdAsync?storeId={2}" +
                                  "&categoryId={3}" +
                                  "&typeName={4}" +
                                  "&isActive={5}&page={6}&pageSize={7}&search={8}", WebServiceAddress, ApiControllerName, storeId, categoryId, typeName, isActive, page, pageSize, search);
                return HttpRequestHelper.GetUrlPagedResultsAsync<Product>(url);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                return null;
            }

        }

        public Task<Product> GetProductsByIdAsync(int productId)
        {
            try
            {
                SetCache();
                string url = string.Format("http://{0}/api/{1}/GetProductsByIdAsync?productId={2}", WebServiceAddress, ApiControllerName, productId);
                return HttpRequestHelper.GetUrlResultAsync<Product>(url);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                return null;
            }


        }

        public Task<List<Product>> GetMainPageProductsAsync(int storeId, int? take)
        {

            try
            {
                SetCache();
                string url = string.Format("http://{0}/api/{1}/GetMainPageProductsAsync?" +
                                                 "storeId={2}" +
                                                 "&take={3}",
                                                 WebServiceAddress,
                                                 ApiControllerName,
                                                 storeId,
                                                 take);

                return HttpRequestHelper.GetUrlResultsAsync<Product>(url);


            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                return null;
            }
        }

        public Task<List<Product>> GetProductsAsync(int storeId, int? take, bool? isActive)
        {
            try
            {
                SetCache();
                string url = string.Format("http://{0}/api/{1}/GetProductsAsync?" +
                                                 "storeId={2}" +
                                                 "&take={3}&isActive={4}",
                                                 WebServiceAddress,
                                                 ApiControllerName,
                                                 storeId,
                                                 take, isActive);

                return HttpRequestHelper.GetUrlResultsAsync<Product>(url);


            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                return null;
            }
        }

        public Task<List<Product>> GetProductByTypeAndCategoryIdAsync(int storeId, int categoryId, int? take, int? excludedProductId)
        {
            try
            {
                SetCache();
                string url = string.Format("http://{0}/api/{1}/GetProductByTypeAndCategoryIdAsync?" +
                                                 "storeId={2}&categoryId={3}" +
                                                 "&take={4}&excludedProductId={5}",
                                                 WebServiceAddress,
                                                 ApiControllerName,
                                                 storeId, categoryId,
                                                 take, excludedProductId);

                return HttpRequestHelper.GetUrlResultsAsync<Product>(url);


            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                return null;
            }
        }

        public Task<List<Product>> GetProductsByBrandAsync(int storeId, int brandId, int? take, int? excludedProductId)
        {
            try
            {
                SetCache();
                string url = string.Format("http://{0}/api/{1}/GetProductsByBrandAsync?" +
                                                 "storeId={2}&brandId={3}" +
                                                 "&take={4}&excludedProductId={5}",
                                                 WebServiceAddress,
                                                 ApiControllerName,
                                                 storeId, brandId,
                                                 take, excludedProductId);

                return HttpRequestHelper.GetUrlResultsAsync<Product>(url);


            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                return null;
            }
        }

        public Task<List<Product>> GetProductsByRetailerAsync(int storeId, int retailerId, int? take, int? excludedProductId)
        {
            try
            {
                SetCache();
                string url = string.Format("http://{0}/api/{1}/GetProductsByRetailerAsync?" +
                                                 "storeId={2}&retailerId={3}" +
                                                 "&take={4}&excludedProductId={5}",
                                                 WebServiceAddress,
                                                 ApiControllerName,
                                                 storeId, retailerId,
                                                 take, excludedProductId);

                return HttpRequestHelper.GetUrlResultsAsync<Product>(url);


            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                return null;
            }
        }

        public Task<List<Product>> GetProductsByProductType(int storeId, int? categoryId, int? brandId, int? retailerId, string productType, int page, int pageSize, bool? isActive, string functionType, int? excludedProductId)
        {
            try
            {
                SetCache();
                string url = string.Format("http://{0}/api/{1}/GetProductsByProductType?" +
                                                    "storeId={2}&categoryId={3}&brandId={4}&productType={5}" +
                                                    "&page={6}&pageSize={7}&isActive={8}&functionType={9}&excludedProductId={10}&retailerId={11}",
                                                    WebServiceAddress,
                                                    ApiControllerName,
                                                    storeId, categoryId, brandId, productType,
                                                    page, pageSize, isActive, functionType, excludedProductId, retailerId);

                return HttpRequestHelper.GetUrlResultsAsync<Product>(url);


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
