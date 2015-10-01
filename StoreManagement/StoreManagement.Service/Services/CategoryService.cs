using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.HelpersModel;
using StoreManagement.Data.Paging;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Services
{
    public class CategoryService : BaseService, ICategoryService
    {

        protected override string ApiControllerName { get { return "Categories"; } }


        public CategoryService(string webServiceAddress)
            : base(webServiceAddress)
        {

        }

        
        public List<Category> GetCategoriesByStoreId(int storeId)
        {
            string url = string.Format("http://{0}/api/{1}/GetCategoriesByStoreId?storeId={2}", WebServiceAddress, ApiControllerName, storeId);
            SetCache();
            return HttpRequestHelper.GetUrlResults<Category>(url);

        }

        public List<Category> GetCategoriesByStoreIdWithContent(int storeId, int? take)
        {

            string url = string.Format("http://{0}/api/{1}/GetCategoriesByStoreId?storeId={2}&take={3}", WebServiceAddress, ApiControllerName, storeId, take);
            SetCache();
            return HttpRequestHelper.GetUrlResults<Category>(url);

        }



        public List<Category> GetCategoriesByStoreId(int storeId, string type, bool? isActive)
        {

            string url = string.Format("http://{0}/api/{1}/GetCategoriesByStoreId?storeId={2}&type={3}&isActive={4}", WebServiceAddress, ApiControllerName, storeId, type, isActive);
            SetCache();
            return HttpRequestHelper.GetUrlResults<Category>(url);

        }

        public List<Category> GetCategoriesByStoreId(int storeId, string type, string search)
        {
            throw new NotImplementedException();
        }

        public List<Category> GetCategoriesByType(string type)
        {
            throw new NotImplementedException();
        }

        public List<Category> GetCategoriesByStoreIdFromCache(int storeId, string type)
        {

            string url = string.Format("http://{0}/api/{1}/GetCategoriesByStoreIdFromCache?storeId={2}&type={3}", WebServiceAddress, ApiControllerName, storeId, type);
            SetCache();
            return HttpRequestHelper.GetUrlResults<Category>(url);
        }


        public Category GetCategory(int id)
        {
            string url = string.Format("http://{0}/api/{1}/GetCategory?id={2}", WebServiceAddress, ApiControllerName, id);
            SetCache();
            return HttpRequestHelper.GetUrlResult<Category>(url);
        }

        public StorePagedList<Category> GetCategoryWithContents(int categoryId, int page, int pageSize = 25)
        {
            string url = string.Format("http://{0}/api/{1}/GetCategoryWithContents?categoryId={2}&page={3}&pageSize={4}", WebServiceAddress, ApiControllerName, categoryId, page, pageSize);
            SetCache();
            return HttpRequestHelper.GetUrlPagedResults<Category>(url);
        }

        public Task<StorePagedList<Category>> GetCategoryWithContentsAsync(int categoryId, int page, int pageSize = 25)
        {
            string url = string.Format("http://{0}/api/{1}/GetCategoryWithContentsAsync?categoryId={2}&page={3}&pageSize={4}", WebServiceAddress, ApiControllerName, categoryId, page, pageSize);
            SetCache();
            return HttpRequestHelper.GetUrlPagedResultsAsync<Category>(url);
        }
 

        public Task<List<Category>> GetCategoriesByStoreIdAsync(int storeId, string type, bool? isActive)
        {
            string url = string.Format("http://{0}/api/{1}/GetCategoriesByStoreIdAsync?storeId={2}&type={3}&isActive={4}",
                WebServiceAddress,
                ApiControllerName,
                storeId,
                type,
                isActive.HasValue ? isActive.Value.ToStr() : "");
            SetCache();
            return HttpRequestHelper.GetUrlResultsAsync<Category>(url);
        }

        public Task<Category> GetCategoryAsync(int id)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetCategoryAsync?id={2}", WebServiceAddress, ApiControllerName, id);
            return HttpRequestHelper.GetUrlResultAsync<Category>(url);
        }

        public Task<Category> GetCategoryByContentIdAsync(int storeId, int contentId)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetCategoryByContentIdAsync?storeId={2}&contentId={3}", WebServiceAddress, ApiControllerName, storeId, contentId);
            return HttpRequestHelper.GetUrlResultAsync<Category>(url);
        }

        public Task<StorePagedList<Category>> GetCategoriesByStoreIdAsync(int storeId, string type, bool? isActive, int page = 1, int pageSize = 25)
        {
            string url = string.Format("http://{0}/api/{1}/GetCategoriesByStoreIdAsync?storeId={2}&type={3}&isActive={4}&page={5}&pageSize={6}",
                    WebServiceAddress,
                    ApiControllerName,
                    storeId,
                    type,
                    isActive, page, pageSize);
            SetCache();
            return HttpRequestHelper.GetUrlPagedResultsAsync<Category>(url);
        }


        protected override void SetCache()
        {
            HttpRequestHelper.CacheMinute = CacheMinute;
            HttpRequestHelper.IsCacheEnable = IsCacheEnable;
        }
    }
}
