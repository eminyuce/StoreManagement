using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;
using StoreManagement.Data;

namespace StoreManagement.Service.Repositories
{
    public class CategoryRepository : EntityRepository<Category, int>, ICategoryRepository
    {

        static TypedObjectCache<List<Category>> CategoryCache
            = new TypedObjectCache<List<Category>>("categoryCache");



        private IStoreContext dbContext;
        public CategoryRepository(IStoreContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<Category> GetCategoriesByStoreId(int storeId, String type)
        {
            return this.FindBy(r => r.StoreId == storeId &&
                r.CategoryType.Equals(type, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        public List<Category> GetCategoriesByStoreIdFromCache(int storeId, String type)
        {
            String key = String.Format("Content-{0}-{1}", storeId, type);
            List<Category> items = null;
            CategoryCache.TryGet(key, out items);

            if (items == null)
            {
                items = GetCategoriesByStoreId(storeId, type);
                CategoryCache.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("Content_CacheAbsoluteExpiration", 10)));
            }

            return items;
        }

        public List<Category> CreateCategoriesTree(int storeId, String type)
        {
            var items = this.GetCategoriesByStoreIdFromCache(storeId, type);
            List<Category> tree = new List<Category>();
            CreateTreeView(tree, items);
            return tree;
        }

        private void CreateTreeView(List<Category> tree, List<Category> items)
        {
            var roots = from s in items where s.ParentId == 0 orderby s.Ordering select s;

            foreach (Category a in roots)
            {
                Category node = new Category();
                node.Name = a.Name;
                node.Ordering = a.Ordering;
                node.Id = a.Id;
                node.ParentId = a.ParentId;
                tree.Add(node);
                CreateChildTree(tree, items, a.ParentId);
            }

        }
        private void CreateChildTree(List<Category> tree, List<Category> items, int parentId)
        {
            var childs = from s in items where s.ParentId == parentId orderby s.Ordering select s;
            foreach (Category a in childs)
            {
                Category node = new Category();
                node.Name = a.Name;
                node.Ordering = a.Ordering;
                node.Id = a.Id;
                node.ParentId = a.ParentId;
                tree.Add(node);
                CreateChildTree(tree, items, a.ParentId);
            }
        }
    }
}
