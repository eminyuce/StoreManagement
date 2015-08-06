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
        private const String ApiControllerName = "Products";

        public ProductService(string webServiceAddress)
            : base(webServiceAddress)
        {

        }


        public Product GetProductsById(int productId)
        {
            string url = string.Format("http://{0}/api/{1}/GetProductsById?productId={2}", WebServiceAddress, ApiControllerName, productId);
            return HttpRequestHelper.GetUrlResult<Product>(url);
        }

        public List<Product> GetProductByType(string typeName)
        {
            string url = string.Format("http://{0}/api/{1}/GetProductByType?typeName={2}", WebServiceAddress, ApiControllerName, typeName);
            return HttpRequestHelper.GetUrlResults<Product>(url);
        }

        public List<Product> GetProductByType(int storeId, string typeName)
        {
            string url = string.Format("http://{0}/api/{1}/GetProductByType?storeId={2}&typeName={3}", WebServiceAddress, ApiControllerName, storeId, typeName);
            return HttpRequestHelper.GetUrlResults<Product>(url);
        }

        public Product GetProductByUrl(int storeId, string url)
        {
            string url2 = string.Format("http://{0}/api/{1}/GetProductByUrl?storeId={2}&url={3}", WebServiceAddress, ApiControllerName, storeId, url);
            return HttpRequestHelper.GetUrlResult<Product>(url2);
        }

        public List<Product> GetProductByTypeAndCategoryId(int storeId, string typeName, int categoryId)
        {
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

        public List<Product> GetProductByTypeAndCategoryId(int storeId, string typeName, int categoryId, string search)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetProductByTypeAndCategoryIdFromCache(int storeId, string typeName, int categoryId)
        {
            string url = string.Format("http://{0}/api/{1}/GetProductByTypeAndCategoryIdFromCache" +
                                       "?storeId={2}" +
                                       "&typeName={3}" +
                                       "&categoryId={4}", WebServiceAddress, ApiControllerName, storeId, typeName, categoryId);
            return HttpRequestHelper.GetUrlResults<Product>(url);

        }

        public StorePagedList<Product> GetProductsCategoryId(int storeId, int? categoryId, string typeName, bool? isActive, int page, int pageSize)
        {

            string url = string.Format("http://{0}/api/{1}/GetProductsCategoryId?storeId={2}" +
                                       "&categoryId={3}" +
                                       "&typeName={4}" +
                                       "&isActive={5}&page={6}&pageSize={7}", WebServiceAddress, ApiControllerName, storeId, categoryId, typeName, isActive, page, pageSize);
            return HttpRequestHelper.GetUrlPagedResults<Product>(url);

        }

        public Product GetProductWithFiles(int id)
        {

            string url = string.Format("http://{0}/api/{1}/GetProductWithFiles?id={2}", WebServiceAddress, ApiControllerName, id);
            return HttpRequestHelper.GetUrlResult<Product>(url);

        }

        public Task<StorePagedList<Product>> GetProductsCategoryIdAsync(int storeId, int? categoryId, string typeName, bool? isActive, int page, int pageSize)
        {
            string url = string.Format("http://{0}/api/{1}/GetProductsCategoryIdAsync?storeId={2}" +
                              "&categoryId={3}" +
                              "&typeName={4}" +
                              "&isActive={5}&page={6}&pageSize={7}", WebServiceAddress, ApiControllerName, storeId, categoryId, typeName, isActive, page, pageSize);
            return HttpRequestHelper.GetUrlPagedResultsAsync<Product>(url);
        }

        public Task<Product> GetProductsByIdAsync(int productId)
        {
            try
            {
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
    }
}
