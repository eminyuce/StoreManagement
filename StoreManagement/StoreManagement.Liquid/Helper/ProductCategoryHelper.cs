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
using StoreManagement.Liquid.Helper.Interfaces;

namespace StoreManagement.Liquid.Helper
{
    

    public class ProductCategoryHelper : BaseLiquidHelper, IProductCategoryHelper
    {


        public StoreLiquidResult GetCategoriesIndexPage(Task<PageDesign> pageDesignTask, Task<StorePagedList<ProductCategory>> categoriesTask)
        {


            Task.WaitAll(pageDesignTask, categoriesTask);
            var pageDesign = pageDesignTask.Result;
            var categories = categoriesTask.Result;


            var result = new StoreLiquidResult();

            try
            {

                if (pageDesign == null)
                {
                    throw new Exception("PageDesing is null");
                }

                var cats = new List<ProductCategoryLiquid>();
                foreach (var item in categories.items)
                {
                    cats.Add(new ProductCategoryLiquid(item, pageDesign));
                }

                object anonymousObject = new
                {
                    categories = LiquidAnonymousObject.GetProductCategories(cats)
                };

                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);


                var dic = new Dictionary<String, String>();
                dic.Add(StoreConstants.PageOutput, indexPageOutput);
                dic.Add(StoreConstants.PageSize, categories.pageSize.ToStr());
                dic.Add(StoreConstants.PageNumber, categories.page.ToStr());
                dic.Add(StoreConstants.TotalItemCount, categories.totalItemCount.ToStr());
                //dic.Add(StoreConstants.IsPagingUp, pageDesign.IsPagingUp ? Boolean.TrueString : Boolean.FalseString);
                //dic.Add(StoreConstants.IsPagingDown, pageDesign.IsPagingDown ? Boolean.TrueString : Boolean.FalseString);

                result.LiquidRenderedResult = dic;

            }
            catch (Exception exception)
            {
                Logger.Error(exception, "GetCategoriesIndexPage : categories and pageDesign", String.Format("Categories Items Count : {0}", categories.items.Count));
            }

            return result;

        }

        public StoreLiquidResult GetProductCategoriesPartial(Task<List<ProductCategory>> categoriesTask, Task<PageDesign> pageDesignTask)
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
                    categories = LiquidAnonymousObject.GetProductCategories(cats)
                };

                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);
                dic[StoreConstants.PageOutput] = indexPageOutput;


            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetProductCategoriesPartial");
            }

            var result = new StoreLiquidResult();
            result.LiquidRenderedResult = dic;
            return result;
        }

        public StoreLiquidResult GetCategoryPage(Task<PageDesign> pageDesignTask, Task<ProductCategory> categoriesTask, Task<StorePagedList<Product>> productsTask)
        {
            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, "");
            try
            {
                Task.WaitAll(pageDesignTask, categoriesTask, productsTask);
                var pageDesign = pageDesignTask.Result;
                var category = categoriesTask.Result;
                var products = productsTask.Result;

                var productCategories = new ProductCategoryLiquid(category, pageDesign);

                var items = new List<ProductLiquid>();
                foreach (var item in products.items)
                {
                    var blog = new ProductLiquid(item, category, pageDesign, ImageWidth, ImageHeight);
                    items.Add(blog);
                }

                object anonymousObject = new
                {
                    category = LiquidAnonymousObject.GetProductCategory(productCategories),
                    products = LiquidAnonymousObject.GetProductsLiquid(items)
                };

                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);
                dic[StoreConstants.PageOutput] = indexPageOutput;
                dic.Add(StoreConstants.PageSize, products.pageSize.ToStr());
                dic.Add(StoreConstants.PageNumber, products.page.ToStr());
                dic.Add(StoreConstants.TotalItemCount, products.totalItemCount.ToStr());
                //dic.Add(StoreConstants.IsPagingUp, pageDesign.IsPagingUp ? Boolean.TrueString : Boolean.FalseString);
               // dic.Add(StoreConstants.IsPagingDown, pageDesign.IsPagingDown ? Boolean.TrueString : Boolean.FalseString);



            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetProductCategoriesPartial");
            }

            var result = new StoreLiquidResult();
            result.LiquidRenderedResult = dic;
            return result;
        }
    }
}