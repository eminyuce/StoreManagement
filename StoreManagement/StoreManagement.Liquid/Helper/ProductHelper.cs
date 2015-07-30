using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.LiquidEngineHelpers;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.Paging;

namespace StoreManagement.Liquid.Helper
{
    public class ProductHelper
    {
        public static Dictionary<string, string> GetProductsIndexPage(Task<StorePagedList<Product>> productsTask,
            Task<PageDesign> pageDesignTask, Task<List<ProductCategory>> categoriesTask)
        {

            Task.WaitAll(pageDesignTask, productsTask, categoriesTask);
            var products = productsTask.Result;
            var pageDesign = pageDesignTask.Result;
            var categories = categoriesTask.Result;
            var items = new List<ProductLiquid>();
            var cats = new List<CategoryLiquid>();
            foreach (var item in products.items)
            {
                var category = categories.FirstOrDefault(r => r.Id == item.ProductCategoryId);
                if (category != null)
                {
                    var blog = new ProductLiquid(item, category, pageDesign);
                    items.Add(blog);
                }
                var catLiquid = new CategoryLiquid(category, pageDesign);
                cats.Add(catLiquid);

            }

            object anonymousObject = new
                {
                    items = from s in items
                            select new
                                {
                                    s.Product.Name,
                                    s.Product.Description,
                                    s.DetailLink,
                                    images = s.ImageLiquid 
                                },
                    categories = from s in cats
                                 select new
                                     {
                                         s.Category.Name,
                                         s.Category.Description,
                                         s.DetailLink,
                                     }
                };

            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);


            var dic = new Dictionary<String, String>();
            dic.Add("PageOutput", indexPageOutput);
            dic.Add("PageSize", products.pageSize.ToStr());
            dic.Add("PageNumber", (products.page - 1).ToStr());
            dic.Add("TotalItemCount", products.totalItemCount.ToStr());
            dic.Add("IsPagingUp", pageDesign.IsPagingUp ? Boolean.TrueString : Boolean.FalseString);
            dic.Add("IsPagingDown", pageDesign.IsPagingDown ? Boolean.TrueString : Boolean.FalseString);


            return dic;
        }

    }
}