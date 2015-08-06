using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using NLog;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.LiquidEngineHelpers;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.Paging;

namespace StoreManagement.Liquid.Helper
{
    public class ProductCategoryHelper : BaseLiquidHelper
    {
        public Dictionary<String, String> GetCategoriesIndexPage(Task<PageDesign> pageDesignTask, Task<StorePagedList<ProductCategory>> categoriesTask)
        {
            Task.WaitAll(pageDesignTask, categoriesTask);
            var pageDesign = pageDesignTask.Result;
            var categories = categoriesTask.Result;

          
            var cats = new List<ProductCategoryLiquid>();
            foreach (var item in categories.items)
            {
                cats.Add(new ProductCategoryLiquid(item, pageDesign));
            }

            object anonymousObject = new
            {
                categories = from s in cats
                             select new
                             {
                                 s.ProductCategory.Name,
                                 s.ProductCategory.Description,
                                 s.DetailLink,
                             }
            };

            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);


            var dic = new Dictionary<String, String>();
            dic.Add("PageOutput", indexPageOutput);
            dic.Add("PageSize", categories.pageSize.ToStr());
            dic.Add("PageNumber", (categories.page - 1).ToStr());
            dic.Add("TotalItemCount", categories.totalItemCount.ToStr());
            dic.Add("IsPagingUp", pageDesign.IsPagingUp ? Boolean.TrueString : Boolean.FalseString);
            dic.Add("IsPagingDown", pageDesign.IsPagingDown ? Boolean.TrueString : Boolean.FalseString);


            return dic;

        }
    }
}