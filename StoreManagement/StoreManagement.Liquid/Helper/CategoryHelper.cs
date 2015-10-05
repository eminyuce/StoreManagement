using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.LiquidEngineHelpers;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.Paging;
using StoreManagement.Liquid.Helper.Interfaces;

namespace StoreManagement.Liquid.Helper
{
    public class CategoryHelper : BaseLiquidHelper, ICategoryHelper
    {
        public StoreLiquidResult GetCategoriesIndexPage(PageDesign pageDesign, StorePagedList<Category> categories, string type)
        {
            var result = new StoreLiquidResult();

            try
            {



                var cats = new List<CategoryLiquid>();
                foreach (var item in categories.items)
                {
                    cats.Add(new CategoryLiquid(item, type));
                }

                object anonymousObject = new
                {
                    categories = LiquidAnonymousObject.GetCategoriesLiquid(cats)
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

        public StoreLiquidResult GetCategoryPage(PageDesign pageDesign, Category category, StorePagedList<Content> contents, String type)
        {
            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, "");
            try
            {

                var contentCategory = new CategoryLiquid(category, type);

                var items = new List<ContentLiquid>();
                foreach (var item in contents.items)
                {
                    var blog = new ContentLiquid(item, category, type, ImageWidth, ImageHeight);
                    items.Add(blog);
                }

                object anonymousObject = new
                {
                    category = LiquidAnonymousObject.GetCategory(contentCategory),
                    items = LiquidAnonymousObject.GetContentLiquid(items)
                };

                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);
                dic[StoreConstants.PageOutput] = indexPageOutput;
                dic.Add(StoreConstants.PageSize, contents.pageSize.ToStr());
                dic.Add(StoreConstants.PageNumber, contents.page.ToStr());
                dic.Add(StoreConstants.TotalItemCount, contents.totalItemCount.ToStr());
                //dic.Add(StoreConstants.IsPagingUp, pageDesign.IsPagingUp ? Boolean.TrueString : Boolean.FalseString);
                // dic.Add(StoreConstants.IsPagingDown, pageDesign.IsPagingDown ? Boolean.TrueString : Boolean.FalseString);



            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetCategoriesPartial");
            }

            var result = new StoreLiquidResult();
            result.LiquidRenderedResult = dic;
            return result;
        }
    }
}