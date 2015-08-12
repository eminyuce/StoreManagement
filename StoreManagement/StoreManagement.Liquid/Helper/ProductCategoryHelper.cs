using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using NLog;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.LiquidEngineHelpers;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.Paging;

namespace StoreManagement.Liquid.Helper
{
    public class ProductCategoryHelper : BaseLiquidHelper
    {


        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }



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
            dic.Add(StoreConstants.PageOutput, indexPageOutput);
            dic.Add(StoreConstants.PageSize, categories.pageSize.ToStr());
            dic.Add(StoreConstants.PageNumber, categories.page.ToStr());
            dic.Add(StoreConstants.TotalItemCount, categories.totalItemCount.ToStr());
            dic.Add(StoreConstants.IsPagingUp, pageDesign.IsPagingUp ? Boolean.TrueString : Boolean.FalseString);
            dic.Add(StoreConstants.IsPagingDown, pageDesign.IsPagingDown ? Boolean.TrueString : Boolean.FalseString);


            return dic;

        }

        public Dictionary<string, string> GetProductCategoriesPartial(Task<List<ProductCategory>> categoriesTask, Task<PageDesign> pageDesignTask)
        {
            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, "");
            try
            {
                Task.WaitAll(pageDesignTask, categoriesTask);
                var pageDesign = pageDesignTask.Result;
                var categories = categoriesTask.Result;


                var cats = new List<ProductCategoryLiquid>();
                foreach (var item in categories)
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
                dic[StoreConstants.PageOutput] = indexPageOutput;
        

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetProductCategoriesPartial");
            }

            return dic;
        }
    }
}