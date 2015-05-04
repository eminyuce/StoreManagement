using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.JsTree;
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

        public List<JsTreeNode> CreateCategoriesTree(int storeId, String type)
        {
            var items = this.GetCategoriesByStoreIdFromCache(storeId, type);
            List<JsTreeNode> tree = new List<JsTreeNode>();
            CreateTreeView(tree, items);
            return tree;
        }

        private void CreateTreeView(List<JsTreeNode> tree, List<Category> items)
        {
            var roots = from s in items where s.ParentId == 0 orderby s.Ordering select s;

            foreach (Category a in roots)
            {
                JsTreeNode node = new JsTreeNode();
                node.attributes = new Attributes();
                node.attributes.id = "rootnod" + a.Id;
                node.attributes.rel = "root" + a.Id;
                node.data = new StoreManagement.Data.JsTree.Data();
                node.data.title = a.Name;
                node.state = "open";
                node.attributes.mdata = "{draggable : false,max_children : 1, max_depth :1}";
                tree.Add(node);
                CreateChildTree(node, items, a);
            }

        }
        private void CreateChildTree(JsTreeNode parentTreeNode, List<Category> items, Category parentNode)
        {
            var childs = from s in items where s.ParentId == parentNode.Id orderby s.Ordering select s;
            if (childs.Any())
            {
                parentTreeNode.children = new List<JsTreeNode>();
                foreach (Category a in childs)
                {
                    JsTreeNode node = new JsTreeNode();
                    node.attributes = new Attributes();
                    node.attributes.id = "rootnod" + a.Id;
                    node.attributes.rel = "root" + a.Id;
                    node.data = new StoreManagement.Data.JsTree.Data();
                    node.data.title = a.Name;
                    node.state = "open";
                    node.attributes.mdata = "{draggable : true,max_children : 1,max_depth : 1 }";
                    parentTreeNode.children.Add(node);
                    CreateChildTree(node, items, a);
                }

            }

        }
    }
}
