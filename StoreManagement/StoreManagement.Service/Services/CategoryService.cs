using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
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
            throw new NotImplementedException();
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
