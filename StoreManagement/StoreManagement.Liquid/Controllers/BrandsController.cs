using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NLog;
using StoreManagement.Data.Constants;
using StoreManagement.Data.GeneralHelper;


namespace StoreManagement.Liquid.Controllers
{
    public class BrandsController : BaseController
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private const String PageDesingIndexPageName = "BrandsIndexPage";
        private const String BrandDetailPageDesignName = "BrandDetailPage";

        public async Task<ActionResult> Index(int page = 1)
        {
            try
            {


                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, PageDesingIndexPageName);
                var pageSize = GetSettingValueInt("Brands_PageSize", StoreConstants.DefaultPageSize);
                var brandsTask = BrandService.GetBrandsByStoreIdWithPagingAsync(StoreId, true, page, pageSize);
                var settings = GetStoreSettings();

                await Task.WhenAll(pageDesignTask, brandsTask);
                var pageDesign = pageDesignTask.Result;
                var brands = brandsTask.Result;
                if (pageDesign == null)
                {
                    Logger.Error("PageDesing is null:" + PageDesingIndexPageName);
                    throw new Exception("PageDesing is null:" + PageDesingIndexPageName);
                }
                var pageOutput = BrandService2.GetBrandsIndexPage(pageDesign, brands);
                var pagingPageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "Paging");


                PagingService2.PageOutput = pageOutput;
                PagingService2.HttpRequestBase = this.Request;
                PagingService2.RouteData = this.RouteData;
                PagingService2.ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                PagingService2.ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                await Task.WhenAll(pagingPageDesignTask);
                var pagingDic = PagingService2.GetPaging(pagingPageDesignTask.Result);
                pagingDic.StoreSettings = settings;
                pageOutput.MyStore = this.MyStore;
                pageOutput.PageTitle = "Brands";
                return View(pagingDic);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Brands:Index:" + ex.StackTrace, page);
                return new HttpStatusCodeResult(500);
            }
        }
        [OutputCache(CacheProfile = "Cache20Minutes")]
        public async Task<ActionResult> Detail(String id = "")
        {
            try
            {

                int brandId = id.Split("-".ToCharArray()).Last().ToInt();
                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, BrandDetailPageDesignName);
                var brandTask = BrandService.GetBrandAsync(brandId);
                var take = GetSettingValueInt("BrandProducts_ItemNumber", 20);
                var productsTask = ProductService.GetProductsByBrandAsync(StoreId, brandId, take, 0);
                var productCategoriesTask = ProductCategoryService.GetCategoriesByBrandIdAsync(StoreId, brandId);

                var settings = GetStoreSettings();

                BrandService2.ImageWidth = GetSettingValueInt("BrandDetail_ImageWidth", 50);
                BrandService2.ImageHeight = GetSettingValueInt("BrandDetail_ImageHeight", 50);

                await Task.WhenAll(brandTask, pageDesignTask, productsTask, productCategoriesTask);
                var pageDesign = pageDesignTask.Result;
                var products = productsTask.Result;
                var productCategories = productCategoriesTask.Result;
                var brand = brandTask.Result;

                if (pageDesign == null)
                {
                    throw new Exception("PageDesing is null:" + BrandDetailPageDesignName);
                }

                var dic = BrandService2.GetBrandDetailPage(brand, products, pageDesign, productCategories);
                dic.StoreSettings = settings;
                dic.MyStore = this.MyStore;
                dic.PageTitle = brand.Name;
                return View(dic);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Stack Trace:" + ex.StackTrace, id);
                return new HttpStatusCodeResult(500);
            }
        }
    }
}