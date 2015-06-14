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


        public Product GetProductsProductId(int productId)
        {
            try
            {
                string url = string.Format("http://{0}/api/{1}/GetProductsProductId?productId={2}", WebServiceAddress, ApiControllerName, productId);
                return HttpRequestHelper.GetUrlResult<Product>(url);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error:" + ex.Message, ex);
                return new Product();
            }
        }

        public List<Product> GetProductByType(string typeName)
        {
            try
            {
                string url = string.Format("http://{0}/api/{1}/GetProductByType?typeName={2}", WebServiceAddress, ApiControllerName, typeName);
                return HttpRequestHelper.GetUrlResults<Product>(url);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error:" + ex.Message, ex);
                return new List<Product>();
            }
        }

        public List<Product> GetProductByType(int storeId, string typeName)
        {
            try
            {
                string url = string.Format("http://{0}/api/{1}/GetProductByType?storeId={2}&typeName={3}", WebServiceAddress, ApiControllerName, storeId, typeName);
                return HttpRequestHelper.GetUrlResults<Product>(url);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error:" + ex.Message, ex);
                return new List<Product>();
            }
        }

        public Product GetProductByUrl(int storeId, string url)
        {
            try
            {
                string url2 = string.Format("http://{0}/api/{1}/GetProductByUrl?storeId={2}&url={3}", WebServiceAddress, ApiControllerName, storeId, url);
                return HttpRequestHelper.GetUrlResult<Product>(url2);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error:" + ex.Message, ex);
                return new Product();
            }
        }

        public List<Product> GetProductByTypeAndCategoryId(int storeId, string typeName, int categoryId)
        {
            try
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
            catch (Exception ex)
            {
                Logger.ErrorException("Error:" + ex.Message, ex);
                return new List<Product>();
            }
        }

        public List<Product> GetProductByTypeAndCategoryIdFromCache(int storeId, string typeName, int categoryId)
        {
            try
            {
                string url = string.Format("http://{0}/api/{1}/GetProductByTypeAndCategoryIdFromCache" +
                                           "?storeId={2}" +
                                           "&typeName={3}" +
                                           "&categoryId={4}", WebServiceAddress, ApiControllerName, storeId, typeName, categoryId);
                return HttpRequestHelper.GetUrlResults<Product>(url);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error:" + ex.Message, ex);
                return new List<Product>();
            }
        }

        public StorePagedList<Product> GetProductsCategoryId(int storeId, int? categoryId, string typeName, bool? isActive, int page, int pageSize)
        {
            try
            {
                string url = string.Format("http://{0}/api/{1}/GetProductsCategoryId?storeId={2}" +
                                           "&categoryId={3}" +
                                           "&typeName={4}" +
                                           "&isActive={5}&page={6}&pageSize={7}", WebServiceAddress, ApiControllerName, storeId, categoryId, typeName, isActive, page, pageSize);
                return HttpRequestHelper.GetUrlPagedResults<Product>(url);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error:" + ex.Message, ex);
                return null;
            }
        }

        public Product GetProductWithFiles(int id)
        {
            try
            {
                string url = string.Format("http://{0}/api/{1}/GetProductWithFiles?id={2}", WebServiceAddress, ApiControllerName, id);

                return HttpRequestHelper.GetUrlResult<Product>(url);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error:" + ex.Message, ex);
                return new Product();
            }
        }
    }
}
