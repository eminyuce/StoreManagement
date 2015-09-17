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
    [OutputCache(CacheProfile = "Cache1Hour")]
    public class AjaxProductsController : BaseController
    {
        public async Task<JsonResult> GetProductCategories(String desingName = "")
        {

            if (String.IsNullOrEmpty(desingName))
            {
                desingName = GetSettingValue("ProductCategoriesPartial_DefaultPageDesign", "ProductCategoriesPartial");
            }
            String returtHtml = "";

            try
            {
                var categoriesTask = ProductCategoryService.GetProductCategoriesByStoreIdAsync(StoreId, StoreConstants.ProductType, true);
                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, desingName);

                ProductCategoryHelper.StoreSettings = GetStoreSettings();
                ProductCategoryHelper.ImageWidth = GetSettingValueInt("ProductCategoriesPartial_ImageWidth", 50);
                ProductCategoryHelper.ImageHeight = GetSettingValueInt("ProductCategoriesPartial_ImageHeight", 50);

                await Task.WhenAll(categoriesTask, pageDesignTask);
                var categories = categoriesTask.Result;
                var pageDesign = pageDesignTask.Result;

                var pageOuput = ProductCategoryHelper.GetProductCategoriesPartial(categories, pageDesign);
                returtHtml = pageOuput.PageOutputText;

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetProductCategories");

            }



            return Json(returtHtml, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetRelatedProductsPartialByCategory(int categoryId, int excludedProductId = 0, String desingName = "")
        {
            if (String.IsNullOrEmpty(desingName))
            {
                desingName = "RelatedProductsPartialByCategory";
            }
            String returtHtml = "";
            try
            {
                var categoryTask = ProductCategoryService.GetProductCategoryAsync(categoryId);
                int take = GetSettingValueInt("RelatedProducts_ItemsNumber", 5);
                var relatedProductsTask = ProductService.GetProductByTypeAndCategoryIdAsync(StoreId, categoryId, take, excludedProductId);
                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, desingName);

                ProductHelper.StoreSettings = GetStoreSettings();
                ProductHelper.ImageWidth = GetSettingValueInt("RelatedProductsPartialByCategory_ImageWidth", 50);
                ProductHelper.ImageHeight = GetSettingValueInt("RelatedProductsPartialByCategory_ImageHeight", 50);

                Task.WhenAll(pageDesignTask, relatedProductsTask, categoryTask);
                var products = relatedProductsTask.Result;
                var pageDesign = pageDesignTask.Result;
                var category = categoryTask.Result;

                var pageOuput = ProductHelper.GetRelatedProductsPartialByCategory(category, products, pageDesign);
                returtHtml = pageOuput.PageOutputText;

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetRelatedProducts", categoryId);

            }

            return Json(returtHtml, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetRelatedProductsPartialByBrand(int brandId, int excludedProductId = 0, String desingName = "")
        {
            if (String.IsNullOrEmpty(desingName))
            {
                desingName = "RelatedProductsPartialByBrand";
            }
            String returtHtml = "";
            try
            {
                var productCategoriesTask = ProductCategoryService.GetProductCategoriesByStoreIdAsync(StoreId,
                                                                                             StoreConstants
                                                                                                 .ProductType, true);
                var brandTask = BrandService.GetBrandAsync(brandId);
                int take = GetSettingValueInt("RelatedProducts_ItemsNumber", 5);
                var relatedProductsTask = ProductService.GetProductsByBrandAsync(StoreId, brandId, take, excludedProductId);
                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, desingName);

                ProductHelper.StoreSettings = GetStoreSettings();
                ProductHelper.ImageWidth = GetSettingValueInt("RelatedProductsPartialByBrand_ImageWidth", 50);
                ProductHelper.ImageHeight = GetSettingValueInt("RelatedProductsPartialByBrand_ImageHeight", 50);


                Task.WhenAll(pageDesignTask, relatedProductsTask, brandTask, productCategoriesTask);
                var products = relatedProductsTask.Result;
                var pageDesign = pageDesignTask.Result;
                var brand = brandTask.Result;
                var productCategories = productCategoriesTask.Result;


                var pageOuput = ProductHelper.GetRelatedProductsPartialByBrand(brand, products, pageDesign, productCategories);
                returtHtml = pageOuput.PageOutputText;

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetRelatedProducts", brandId);

            }


            return Json(returtHtml, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetBrands(String desingName = "")
        {

            if (String.IsNullOrEmpty(desingName))
            {
                desingName = GetSettingValue("BrandsPartial_DefaultPageDesign", "BrandsPartial");
            }

            String returtHtml = "";

            try
            {
                int take = GetSettingValueInt("BrandsPartial_ItemsNumber", 50);
                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, desingName);
                var brandsTask = BrandService.GetBrandsAsync(StoreId, take, true);

                BrandHelper.StoreSettings = GetStoreSettings();
                BrandHelper.ImageWidth = GetSettingValueInt("BrandsPartial_ImageWidth", 50);
                BrandHelper.ImageHeight = GetSettingValueInt("BrandsPartial_ImageHeight", 50);

                await Task.WhenAll(pageDesignTask, brandsTask);
                var pageDesign = pageDesignTask.Result;
                var brands = brandsTask.Result;
                if (pageDesign == null)
                {
                    throw new Exception("PageDesing is null");
                }


                var pageOuput = BrandHelper.GetBrandsPartial(brands, pageDesign);
                returtHtml = pageOuput.PageOutputText;

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetBrands", desingName);

            }


            return Json(returtHtml, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetProductLabels(int id, String designName = "")
        {

            if (String.IsNullOrEmpty(designName))
            {
                designName = GetSettingValue("ProductLabels_DefaultPageDesign", "ProductLabelsPartial");
            }

            String returtHtml = "";

            try
            {
                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, designName);
                var labelsTask = LabelService.GetLabelsByItemTypeId(StoreId, id, StoreConstants.ProductType);

                LabelHelper.StoreSettings = GetStoreSettings();
                LabelHelper.ImageWidth = GetSettingValueInt("ProductLabels_ImageWidth", 50);
                LabelHelper.ImageHeight = GetSettingValueInt("ProductLabels_ImageHeight", 50);


                await Task.WhenAll(pageDesignTask, labelsTask);
                var labels = labelsTask.Result;
                var pageDesign = pageDesignTask.Result;

                if (pageDesign == null)
                {
                    throw new Exception("PageDesing is null");
                }



                var pageOuput = LabelHelper.GetProductLabels(labels, pageDesign);
                returtHtml = pageOuput.PageOutputText;

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetProductLabels", designName);
            }


            return Json(returtHtml, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetPopularProducts(int page = 1, String desingName = "", int categoryId = -1, int brandId = -1)
        {

            if (String.IsNullOrEmpty(desingName))
            {
                desingName = GetSettingValue("PopularProducts_DefaultPageDesign", "PopularProductsPartial");
            }
            String returtHtml = "";

            try
            {
                int pageSize = GetSettingValueInt("PopularProducts_PageSize", StoreConstants.DefaultPageSize);
                var productsTask = ProductService.GetPopularProducts(StoreId, categoryId == -1 ? (int?)null : categoryId, brandId == -1 ? (int?)null : brandId, StoreConstants.ProductType, page, pageSize, true);
                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, desingName);
                var productCategoriesTask = ProductCategoryService.GetProductCategoriesByStoreIdAsync(StoreId,
                                                                                           StoreConstants
                                                                                               .ProductType, true);

                await Task.WhenAll(pageDesignTask, productsTask, productCategoriesTask);
                var products = productsTask.Result;
                var pageDesign = pageDesignTask.Result;
                var productCategories = productCategoriesTask.Result;

                ProductHelper.StoreSettings = GetStoreSettings();
                ProductHelper.ImageWidth = GetSettingValueInt("PopularProducts_ImageWidth", 99);
                ProductHelper.ImageHeight = GetSettingValueInt("PopularProducts_ImageHeight", 99);
                var pageOuput = ProductHelper.GetPopularProducts(products, productCategories, pageDesign);
                returtHtml = pageOuput.PageOutputText;

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetPopularProducts");

            }


            return Json(returtHtml, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetRecentProducts(int page = 1, String desingName = "", int categoryId = -1, int brandId = -1)
        {

            if (String.IsNullOrEmpty(desingName))
            {
                desingName = GetSettingValue("RecentProducts_DefaultPageDesign", "RecentProductsPartial");
            }

            String returtHtml = "";
            try
            {
                int pageSize = GetSettingValueInt("RecentProducts_PageSize", StoreConstants.DefaultPageSize);
                var productsTask = ProductService.GetRecentProducts(StoreId, categoryId == -1 ? (int?)null : categoryId, brandId == -1 ? (int?)null : brandId, StoreConstants.ProductType, page, pageSize, true);
                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, desingName);
                var productCategoriesTask = ProductCategoryService.GetProductCategoriesByStoreIdAsync(StoreId,
                                                                                           StoreConstants
                                                                                               .ProductType, true);

                await Task.WhenAll(pageDesignTask, productsTask, productCategoriesTask);
                var products = productsTask.Result;
                var pageDesign = pageDesignTask.Result;
                var productCategories = productCategoriesTask.Result;


                ProductHelper.StoreSettings = GetStoreSettings();
                ProductHelper.ImageWidth = GetSettingValueInt("RecentProducts_ImageWidth", 99);
                ProductHelper.ImageHeight = GetSettingValueInt("RecentProducts_ImageHeight", 99);
                var pageOuput = ProductHelper.GetPopularProducts(products, productCategories, pageDesign);
                returtHtml = pageOuput.PageOutputText;

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetRecentProducts");

            }

            return Json(returtHtml, JsonRequestBehavior.AllowGet);
        }



    }
}