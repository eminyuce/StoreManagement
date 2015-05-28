using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
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
       
        public List<Category> GetCategoriesByStoreId(int storeId)
        {
            return this.FindBy(r => r.StoreId == storeId).ToList();

        }

        public List<Category> GetCategoriesByStoreIdWithContent(int storeId)
        {

            //return this.GetAllIncluding(IncludeProperties()).Where(r => r.StoreId == storeId && r.Contents.Any()).OrderByDescending(r => r.Id).Take(10).ToList();
            //IQueryable<Category> mmm = (from z in dbContext.Categories
            //                            join f in dbContext.Contents on z.Id equals f.CategoryId
            //                            join t in dbContext.ContentFiles on f.Id equals t.ContentId
            //                            join y in dbContext.FileManagers on t.FileManagerId equals y.Id
            //                            where z.StoreId == storeId && z.Contents.Any()
            //                            select z).
            //     Include(r => r.Contents.Select(r1 => r1.ContentFiles.Select(m => m.FileManager)))
            //    .Distinct()
            //    .OrderByDescending(r => r.Ordering)
            //    .Take(10);

            //return mmm.ToList();
            return dbContext.Categories.Where(r => r.StoreId == storeId && r.Contents.Any())
                .Include(r => r.Contents.Select(r1 => r1.ContentFiles.Select(m => m.FileManager)))
                .OrderByDescending(r => r.Ordering).Take(10).ToList();
        }

        //public IQueryable<T> Include<T>(params Expression<Func<T, object>>[] paths) where T : class
        //{
        //    IQueryable<T> query = dbContext.Set<T>();
        //    foreach (var path in paths)
        //        query = query.Include(path);
        //    return query;
        //}
        private static Expression<Func<Category, object>> IncludeProperties()
        {
            var param = Expression.Parameter(typeof(Category));
            Expression<Func<Category, object>> exp = r => r.Contents.Select(m => m.ContentFiles.Select(p => p.FileManager));

            return exp;
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
