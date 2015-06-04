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
        private const String ApiControllerName = "Categories";
        public CategoryService(string webServiceAddress)
            : base(webServiceAddress)
        {

        }

        public List<Category> GetCategoriesByStoreId(int storeId)
        {
            try
            {
                string url = string.Format("http://{0}/api/{1}/GetCategoriesByStoreId?storeId={2}", WebServiceAddress, ApiControllerName, storeId);
                return RequestHelper.GetUrlResults<Category>(url);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error:" + ex.Message, ex);
                return new List<Category>();
            }
        }

        public List<Category> GetCategoriesByStoreIdWithContent(int storeId)
        {
            try
            {
                string url = string.Format("http://{0}/api/{1}/GetCategoriesByStoreId?storeId={2}", WebServiceAddress, ApiControllerName, storeId);
                return RequestHelper.GetUrlResults<Category>(url);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error:" + ex.Message, ex);
                return new List<Category>();
            }
        }

        public List<Category> GetCategoriesByStoreId(int storeId, string type)
        {
            try
            {
                string url = string.Format("http://{0}/api/{1}/GetCategoriesByStoreId?storeId={2}", WebServiceAddress, ApiControllerName, storeId);
                return RequestHelper.GetUrlResults<Category>(url);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error:" + ex.Message, ex);
                return new List<Category>();
            }
        }

        public List<Category> GetCategoriesByStoreIdFromCache(int storeId, string type)
        {
            try
            {
                string url = string.Format("http://{0}/api/{1}/GetCategoriesByStoreId?storeId={2}", WebServiceAddress, ApiControllerName, storeId);
                return RequestHelper.GetUrlResults<Category>(url);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error:" + ex.Message, ex);
                return new List<Category>();
            }
        }


        public Category GetSingle(int id)
        {
            try
            {
                string url = string.Format("http://{0}/api/{1}/GetSingle?id={2}", WebServiceAddress, ApiControllerName, id);
                return RequestHelper.GetUrlResult<Category>(url);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error:" + ex.Message, ex);
                return new Category();
            }
        }

        public StorePagedList<Category> GetCategoryWithContents(int categoryId, int page, int pageSize = 25)
        {
            try
            {
                string url = string.Format("http://{0}/api/{1}/GetCategoryWithContents?categoryId={2}&page={3}&pageSize={4}", WebServiceAddress, ApiControllerName, categoryId, page, pageSize);
                return RequestHelper.GetUrlPagedResults<Category>(url);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error:" + ex.Message, ex);
                return null;
            }
        }
    }
}
