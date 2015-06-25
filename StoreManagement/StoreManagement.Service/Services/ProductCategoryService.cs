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
    public class ProductCategoryService : BaseService, IProductCategoryService
    {
        private const String ApiControllerName = "ProductCategories";
        public ProductCategoryService(string webServiceAddress) : base(webServiceAddress)
        {
        }

        public List<ProductCategory> GetProductCategoriesByStoreId(int storeId)
        {
            string url = string.Format("http://{0}/api/{1}/GetProductCategoriesByStoreId?storeId={2}", WebServiceAddress, ApiControllerName, storeId);
            return HttpRequestHelper.GetUrlResults<ProductCategory>(url);
        }

        public List<ProductCategory> GetProductCategoriesByStoreIdWithContent(int storeId)
        {
            string url = string.Format("http://{0}/api/{1}/GetProductCategoriesByStoreIdWithContent?storeId={2}", WebServiceAddress, ApiControllerName, storeId);
            return HttpRequestHelper.GetUrlResults<ProductCategory>(url);
        }

        public List<ProductCategory> GetProductCategoriesByStoreId(int storeId, string type)
        {
            string url = string.Format("http://{0}/api/{1}/GetProductCategoriesByStoreId?storeId={2}&type={3}", WebServiceAddress, ApiControllerName, storeId, type);
            return HttpRequestHelper.GetUrlResults<ProductCategory>(url);
        }

        public List<ProductCategory> GetProductCategoriesByStoreId(int storeId, string type, string search)
        {
            throw new NotImplementedException();
        }

        public List<ProductCategory> GetProductCategoriesByStoreIdFromCache(int storeId, string type)
        {
            string url = string.Format("http://{0}/api/{1}/GetProductCategoriesByStoreIdFromCache?storeId={2}&type={3}", WebServiceAddress, ApiControllerName, storeId, type);
            return HttpRequestHelper.GetUrlResults<ProductCategory>(url);
        }

        public ProductCategory GetProductCategory(int id)
        {
            string url = string.Format("http://{0}/api/{1}/GetProductCategory?id={2}", WebServiceAddress, ApiControllerName, id);
            return HttpRequestHelper.GetUrlResult<ProductCategory>(url);
        }

        public StorePagedList<ProductCategory> GetProductCategoryWithContents(int categoryId, int page, int pageSize = 25)
        {
            string url = string.Format("http://{0}/api/{1}/GetProductCategoryWithContents?categoryId={2}&page={3}&pageSize={4}", WebServiceAddress, ApiControllerName, categoryId, page, pageSize);
            return HttpRequestHelper.GetUrlPagedResults<ProductCategory>(url);
        }
    }
}
