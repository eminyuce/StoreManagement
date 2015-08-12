using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data;
using StoreManagement.Data.Constants;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;
using StoreManagement.Liquid.Helper;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Liquid.Controllers
{
    public class AjaxController : BaseController
    {


        public async Task<JsonResult> GetRelatedContents(int categoryId, String contentType, int excludedContentId = 0)
        {
            var res = Task.Factory.StartNew(() =>
            {
                try
                {


                    var categoryTask = CategoryService.GetCategoryAsync(categoryId);

                    int take = 0;
                    if (contentType.Equals(StoreConstants.NewsType))
                    {
                        take = GetSettingValueInt("RelatedNews_ItemsNumber", 5);
                    }
                    else if (contentType.Equals(StoreConstants.BlogsType))
                    {
                        take = GetSettingValueInt("RelatedBlogs_ItemsNumber", 5);
                    }

                    var relatedContentsTask = ContentService.GetContentByTypeAndCategoryIdAsync(StoreId, contentType, categoryId, take, excludedContentId);
                    var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "RelatedContentsPartial");
                    var liquidHelper = new ContentHelper();
                    liquidHelper.StoreSettings = GetStoreSettings();



                    if (contentType.Equals(StoreConstants.NewsType))
                    {
                        liquidHelper.ImageWidth = GetSettingValueInt("RelatedNewsPartial_ImageWidth", 50);
                        liquidHelper.ImageHeight = GetSettingValueInt("RelatedNewsPartial_ImageHeight", 50);
                    }
                    else if (contentType.Equals(StoreConstants.BlogsType))
                    {
                        liquidHelper.ImageWidth = GetSettingValueInt("RelatedBlogsPartial_ImageWidth", 50);
                        liquidHelper.ImageHeight = GetSettingValueInt("RelatedBlogsPartial_ImageHeight", 50);
                    }
                    else
                    {
                        liquidHelper.ImageWidth = 0;
                        liquidHelper.ImageHeight = 0;
                        Logger.Trace("No ContentType is defined like that " + contentType);
                    }


                    Dictionary<String, String> dic = liquidHelper.GetRelatedContentsPartial(categoryTask, relatedContentsTask, pageDesignTask, contentType);
                    String html = dic[StoreConstants.PageOutput];
                    return html;

                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "GetRelatedContents", contentType, categoryId);
                    return "";
                }

            });

            return Json(await res, JsonRequestBehavior.AllowGet);
        }


        public async Task<JsonResult> GetRelatedProducts(int categoryId, int excludedProductId = 0)
        {

            var res = Task.Factory.StartNew(() =>
            {
                try
                {
                    var categoryTask = ProductCategoryService.GetProductCategoryAsync(categoryId);
                    int take = GetSettingValueInt("RelatedProducts_ItemsNumber", 5);
                    var relatedProductsTask = ProductService.GetProductByTypeAndCategoryIdAsync(StoreId, categoryId, take, excludedProductId);
                    var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "RelatedProductsPartial");
                    var liquidHelper = new ProductHelper();
                    liquidHelper.StoreSettings = GetStoreSettings();
                    liquidHelper.ImageWidth = GetSettingValueInt("RelatedProductsPartial_ImageWidth", 50);
                    liquidHelper.ImageHeight = GetSettingValueInt("RelatedProductsPartial_ImageHeight", 50);
                    Dictionary<String, String> dic = liquidHelper.GetRelatedProductsPartial(categoryTask, relatedProductsTask, pageDesignTask);
                    String html = dic[StoreConstants.PageOutput];
                    return html;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "GetRelatedProducts", categoryId);
                    return "";
                }

            });

            return Json(await res, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetProductCategories()
        {
            var res = Task.Factory.StartNew(() =>
            {
                try
                {
                    var categoriesTask = ProductCategoryService.GetProductCategoriesByStoreIdAsync(StoreId, StoreConstants.ProductType, true);
                    var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "ProductCategoriesPartial");
                    var liquidHelper = new ProductCategoryHelper();
                    liquidHelper.StoreSettings = GetStoreSettings();
                    liquidHelper.ImageWidth = GetSettingValueInt("ProductCategoriesPartial_ImageWidth", 50);
                    liquidHelper.ImageHeight = GetSettingValueInt("ProductCategoriesPartial_ImageHeight", 50);
                    Dictionary<String, String> dic = liquidHelper.GetProductCategoriesPartial(categoriesTask, pageDesignTask);
                    String html = dic[StoreConstants.PageOutput];
                    return html;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "GetProductCategories");
                    return "";
                }

            });

            return Json(await res, JsonRequestBehavior.AllowGet);
        }


    }
}