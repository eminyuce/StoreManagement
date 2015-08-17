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
    public class AjaxProductsController : BaseController
    {
        public async Task<JsonResult> GetProductCategories(String desingName = "")
        {

            if (String.IsNullOrEmpty(desingName))
            {
                desingName = GetSettingValue("ProductCategoriesPartial_DefaultPageDesign", "ProductCategoriesPartial");
            }
            var res = Task.Factory.StartNew(() =>
            {
                try
                {
                    var categoriesTask = ProductCategoryService.GetProductCategoriesByStoreIdAsync(StoreId, StoreConstants.ProductType, true);
                    var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, desingName);
                    var liquidHelper = new ProductCategoryHelper();
                    liquidHelper.StoreSettings = GetStoreSettings();
                    liquidHelper.ImageWidth = GetSettingValueInt("ProductCategoriesPartial_ImageWidth", 50);
                    liquidHelper.ImageHeight = GetSettingValueInt("ProductCategoriesPartial_ImageHeight", 50);
                    var pageOuput = liquidHelper.GetProductCategoriesPartial(categoriesTask, pageDesignTask);
                    String html = pageOuput.PageOutputText;
                    return html;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "GetProductCategories");
                    return "";
                }

            });

            var returtHtml = await res;
            return Json(returtHtml, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetRelatedProductsPartialByCategory(int categoryId, int excludedProductId = 0, String desingName = "")
        {
            if (String.IsNullOrEmpty(desingName))
            {
                desingName = "RelatedProductsPartialByCategory";
            }
            var res = Task.Factory.StartNew(() =>
            {
                try
                {
                    var categoryTask = ProductCategoryService.GetProductCategoryAsync(categoryId);
                    int take = GetSettingValueInt("RelatedProducts_ItemsNumber", 5);
                    var relatedProductsTask = ProductService.GetProductByTypeAndCategoryIdAsync(StoreId, categoryId, take, excludedProductId);
                    var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, desingName);
                    var liquidHelper = new ProductHelper();
                    liquidHelper.StoreSettings = GetStoreSettings();
                    liquidHelper.ImageWidth = GetSettingValueInt("RelatedProductsPartialByCategory_ImageWidth", 50);
                    liquidHelper.ImageHeight = GetSettingValueInt("RelatedProductsPartialByCategory_ImageHeight", 50);
                    var pageOuput = liquidHelper.GetRelatedProductsPartialByCategory(categoryTask, relatedProductsTask, pageDesignTask);
                    String html = pageOuput.PageOutputText;
                    return html;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "GetRelatedProducts", categoryId);
                    return "";
                }

            });

            var returtHtml = await res;
            return Json(returtHtml, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetRelatedProductsPartialByBrand(int brandId, int excludedProductId = 0, String desingName = "")
        {
            if (String.IsNullOrEmpty(desingName))
            {
                desingName = "RelatedProductsPartialByBrand";
            }
            var res = Task.Factory.StartNew(() =>
            {
                try
                {
                    var categoriesTask = ProductCategoryService.GetProductCategoriesByStoreIdAsync(StoreId,
                                                                                                 StoreConstants
                                                                                                     .ProductType, true);
                    var brandTask = BrandService.GetBrandAsync(brandId);
                    int take = GetSettingValueInt("RelatedProducts_ItemsNumber", 5);
                    var relatedProductsTask = ProductService.GetProductByBrandAsync(StoreId, brandId, take, excludedProductId);
                    var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, desingName);
                    var liquidHelper = new ProductHelper();
                    liquidHelper.StoreSettings = GetStoreSettings();
                    liquidHelper.ImageWidth = GetSettingValueInt("RelatedProductsPartialByBrand_ImageWidth", 50);
                    liquidHelper.ImageHeight = GetSettingValueInt("RelatedProductsPartialByBrand_ImageHeight", 50);
                    var pageOuput = liquidHelper.GetRelatedProductsPartialByBrand(brandTask, relatedProductsTask, pageDesignTask, categoriesTask);
                    String html = pageOuput.PageOutputText;
                    return html;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "GetRelatedProducts", brandId);
                    return "";
                }

            });

            var returtHtml = await res;
            return Json(returtHtml, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetBrands(String desingName = "")
        {

            if (String.IsNullOrEmpty(desingName))
            {
                desingName = GetSettingValue("BrandsPartial_DefaultPageDesign", "BrandsPartial");
            }
            var res = Task.Factory.StartNew(() =>
            {
                try
                {
                    var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, desingName);
                    var brandsTask = BrandService.GetBrandsAsync(StoreId, null, true);
                    var liquidHelper = new BrandHelper();
                    liquidHelper.StoreSettings = GetStoreSettings();
                    liquidHelper.ImageWidth = GetSettingValueInt("BrandsPartial_ImageWidth", 50);
                    liquidHelper.ImageHeight = GetSettingValueInt("BrandsPartial_ImageHeight", 50);
                    var pageOuput = liquidHelper.GetBrandsPartial(brandsTask, pageDesignTask);
                    String html = pageOuput.PageOutputText;
                    return html;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "GetBrands", desingName);
                    return "";
                }

            });
            var returtHtml = await res;
            return Json(returtHtml, JsonRequestBehavior.AllowGet);
        }
    }
}