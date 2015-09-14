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

                    ProductCategoryHelper.StoreSettings = GetStoreSettings();
                    ProductCategoryHelper.ImageWidth = GetSettingValueInt("ProductCategoriesPartial_ImageWidth", 50);
                    ProductCategoryHelper.ImageHeight = GetSettingValueInt("ProductCategoriesPartial_ImageHeight", 50);
                    var pageOuput = ProductCategoryHelper.GetProductCategoriesPartial(categoriesTask, pageDesignTask);
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

                    ProductHelper.StoreSettings = GetStoreSettings();
                    ProductHelper.ImageWidth = GetSettingValueInt("RelatedProductsPartialByCategory_ImageWidth", 50);
                    ProductHelper.ImageHeight = GetSettingValueInt("RelatedProductsPartialByCategory_ImageHeight", 50);
                    var pageOuput = ProductHelper.GetRelatedProductsPartialByCategory(categoryTask, relatedProductsTask, pageDesignTask);
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
                    var relatedProductsTask = ProductService.GetProductsByBrandAsync(StoreId, brandId, take, excludedProductId);
                    var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, desingName);

                    ProductHelper.StoreSettings = GetStoreSettings();
                    ProductHelper.ImageWidth = GetSettingValueInt("RelatedProductsPartialByBrand_ImageWidth", 50);
                    ProductHelper.ImageHeight = GetSettingValueInt("RelatedProductsPartialByBrand_ImageHeight", 50);
                    var pageOuput = ProductHelper.GetRelatedProductsPartialByBrand(brandTask, relatedProductsTask, pageDesignTask, categoriesTask);
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
                    int take = GetSettingValueInt("BrandsPartial_ItemsNumber", 50);
                    var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, desingName);
                    var brandsTask = BrandService.GetBrandsAsync(StoreId, take, true);

                    BrandHelper.StoreSettings = GetStoreSettings();
                    BrandHelper.ImageWidth = GetSettingValueInt("BrandsPartial_ImageWidth", 50);
                    BrandHelper.ImageHeight = GetSettingValueInt("BrandsPartial_ImageHeight", 50);
                    var pageOuput = BrandHelper.GetBrandsPartial(brandsTask, pageDesignTask);
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

        public async Task<JsonResult> GetProductLabels(int id, String designName = "")
        {

            if (String.IsNullOrEmpty(designName))
            {
                designName = GetSettingValue("ProductLabels_DefaultPageDesign", "ProductLabelsPartial");
            }
            var res = Task.Factory.StartNew(() =>
            {
                try
                {
                    var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, designName);
                    var brandsTask = LabelService.GetLabelsByItemTypeId(StoreId, id, StoreConstants.ProductType);

                    LabelHelper.StoreSettings = GetStoreSettings();
                    LabelHelper.ImageWidth = GetSettingValueInt("ProductLabels_ImageWidth", 50);
                    LabelHelper.ImageHeight = GetSettingValueInt("ProductLabels_ImageHeight", 50);
                    var pageOuput = LabelHelper.GetProductLabels(brandsTask, pageDesignTask);
                    String html = pageOuput.PageOutputText;
                    return html;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "GetProductLabels", designName);
                    return "";
                }

            });
            var returtHtml = await res;
            return Json(returtHtml, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetPopularProducts(int page = 1, String desingName = "", int categoryId = -1, int brandId = -1)
        {

            if (String.IsNullOrEmpty(desingName))
            {
                desingName = GetSettingValue("PopularProducts_DefaultPageDesign", "PopularProductsPartial");
            }
            var res = Task.Factory.StartNew(() =>
            {
                try
                {
                    int pageSize = GetSettingValueInt("PopularProducts_PageSize", StoreConstants.DefaultPageSize);
                    var productsTask = ProductService.GetPopularProducts(StoreId, categoryId == -1 ? (int?) null : categoryId, brandId ==-1 ? (int?) null : brandId, StoreConstants.ProductType, page, pageSize, true);
                    var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, desingName);
                    var categoriesTask = ProductCategoryService.GetProductCategoriesByStoreIdAsync(StoreId,
                                                                                               StoreConstants
                                                                                                   .ProductType, true);

                    ProductHelper.StoreSettings = GetStoreSettings();
                    ProductHelper.ImageWidth = GetSettingValueInt("PopularProducts_ImageWidth", 99);
                    ProductHelper.ImageHeight = GetSettingValueInt("PopularProducts_ImageHeight", 99);
                    var pageOuput = ProductHelper.GetPopularProducts(productsTask, categoriesTask, pageDesignTask);
                    String html = pageOuput.PageOutputText;
                    return html;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "GetPopularProducts");
                    return ex.Message;
                }

            });

            var returtHtml = await res;
            return Json(returtHtml, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetRecentProducts(int page = 1, String desingName = "",int categoryId=-1, int brandId=-1)
        {

            if (String.IsNullOrEmpty(desingName))
            {
                desingName = GetSettingValue("RecentProducts_DefaultPageDesign", "RecentProductsPartial");
            }
            var res = Task.Factory.StartNew(() =>
            {
                try
                {
                    int pageSize = GetSettingValueInt("RecentProducts_PageSize", StoreConstants.DefaultPageSize);
                    var productsTask = ProductService.GetRecentProducts(StoreId, categoryId == -1 ? (int?)null : categoryId, brandId == -1 ? (int?)null : brandId, StoreConstants.ProductType, page, pageSize, true);
                    var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, desingName);
                    var categoriesTask = ProductCategoryService.GetProductCategoriesByStoreIdAsync(StoreId,
                                                                                               StoreConstants
                                                                                                   .ProductType, true);

                    ProductHelper.StoreSettings = GetStoreSettings();
                    ProductHelper.ImageWidth = GetSettingValueInt("RecentProducts_ImageWidth", 99);
                    ProductHelper.ImageHeight = GetSettingValueInt("RecentProducts_ImageHeight", 99);
                    var pageOuput = ProductHelper.GetPopularProducts(productsTask, categoriesTask, pageDesignTask);
                    String html = pageOuput.PageOutputText;
                    return html;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "GetRecentProducts");
                    return "";
                }

            });

            var returtHtml = await res;
            return Json(returtHtml, JsonRequestBehavior.AllowGet);
        }
      


    }
}