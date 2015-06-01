using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.JsTree;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(string webServiceAddress) : base(webServiceAddress)
        {

        }

        public List<Category> GetCategoriesByStoreId(int storeId)
        {
            try
            {
                string url = string.Format("http://{0}/api/Categories/GetCategoriesByStoreId?storeId={1}", WebServiceAddress, storeId);
                return RequestHelper.GetUrlResults<Category>(url);
            }
            catch (Exception ex)
            {
                WebServiceAddress = string.Empty;
                return new List<Category>();
            }
        }

        public List<Category> GetCategoriesByStoreIdWithContent(int storeId)
        {
            throw new NotImplementedException();
        }

        public List<Category> GetCategoriesByStoreId(int storeId, string type)
        {
            throw new NotImplementedException();
        }

        public List<Category> GetCategoriesByStoreIdFromCache(int storeId, string type)
        {
            throw new NotImplementedException();
        }

        public List<JsTreeNode> CreateCategoriesTree(int storeId, string type)
        {
            throw new NotImplementedException();
        }

        public Category GetSingle(int id)
        {
            throw new NotImplementedException();
        }
    }
}
