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
    [OutputCache(CacheProfile = "Cache1Days")]
    public class ProductsController : BaseController
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private const String IndexPageDesingName = "ProductsIndexPage";
        private const String ProductDetailPage = "ProductDetailPage";


        public async Task<ActionResult> Index(
            String search = "", 
            String filters = "", 
            String page = "",
            String id = "clothes-shoes-and-jewelry")
        {
            search = search.ToStr();
            id = String.IsNullOrEmpty(id) ? "clothes-shoes-and-jewelry" : id;
            String categoryApiId = id;
            RouteData.Values["id"] = id;
            String headerText = "";
            var fltrs = FilterHelper.ParseFiltersFromString(filters);
            if (fltrs.Any())
            {
                headerText = String.Join("– ", fltrs.Select(r => r.Text.ToTitleCase()));
            }

            if (!string.IsNullOrEmpty(search))
            {
                headerText += " '" + search.ToTitleCase() + "'";
            }



            int iPage = page.ToInt(); if (iPage == 0) iPage = 1;
            var pageSize = GetSettingValueInt("ProductsIndex_PageSize", StoreConstants.DefaultPageSize);
            int skip = (iPage - 1) * pageSize;
            var categoriesTask = ProductCategoryService.GetProductCategoriesByStoreIdAsync(StoreId, StoreConstants.ProductType, true);
            var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "ProductsSearchIndexPage");
            var productSearchResultTask = ProductService.GetProductsSearchResult(StoreId, search, filters, pageSize, skip, false, categoryApiId);
            await Task.WhenAll(productSearchResultTask, categoriesTask, pageDesignTask);
            var productSearchResult = productSearchResultTask.Result;
            var pageDesign = pageDesignTask.Result;
            var categories = categoriesTask.Result;

            if (pageDesign == null)
            {
                throw new Exception("PageDesing is null:" + IndexPageDesingName);
            }

            var settings = GetStoreSettings();
            var pageOutput = ProductService2.GetProductsSearchPage(this, productSearchResult, pageDesign, categories, search, filters, headerText, categoryApiId);

            var pagingPageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "Paging");
            PagingService2.PageOutput = pageOutput;
            PagingService2.HttpRequestBase = this.Request;
            PagingService2.RouteData = this.RouteData;
            PagingService2.ActionName = this.ControllerContext.RouteData.Values["action"].ToStr();
            PagingService2.ControllerName = this.ControllerContext.RouteData.Values["controller"].ToStr();
            await Task.WhenAll(pagingPageDesignTask);
            var pagingDic = PagingService2.GetPaging(pagingPageDesignTask.Result);
            pagingDic.StoreSettings = settings;
            pagingDic.PageTitle = pageOutput.PageTitle;
            pagingDic.MyStore = this.MyStore;


            pagingDic.DetailLink = "/products/" + categoryApiId;
            pagingDic.PageTitle = String.IsNullOrEmpty(headerText) ?  pagingDic.PageTitle  : headerText;
            string mmm = 
                GetFilter(productSearchResult.Filters, "category", 4) + " " +
                GetFilter(productSearchResult.Filters, "brand", 20);
            ViewData[StoreConstants.MetaTagKeywords] = pagingDic.PageTitle+", "+mmm;
            ViewData[StoreConstants.MetaTagDescription] = GeneralHelper.TruncateAtWord(pagingDic.PageTitle + ", " + mmm, 155);
            
            return View(pagingDic);



        }

        private String GetFilter(List<Data.HelpersModel.Filter> list, string fieldName, int totalItem)
        {
            var listSp =
                list.Where(r => r.FieldName.Equals(fieldName, StringComparison.InvariantCultureIgnoreCase))
                    .OrderByDescending(r => r.Cnt)
                    .Take(totalItem)
                    .Select(r => r.Text);

            return String.Join(", ", listSp);
        }
        
        public async Task<ActionResult> Index3(int page = 1, int catId = 0, String search = "", String filters = "")
        {
            try
            {
                if (!IsModulActive(StoreConstants.ProductType))
                {
                    return HttpNotFound("Not Found");
                }

                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, IndexPageDesingName);
                var pageSize = GetSettingValueInt("ProductsIndex_PageSize", StoreConstants.DefaultPageSize);
                var productsTask = ProductService.GetProductsCategoryIdAsync(StoreId, catId, StoreConstants.ProductType, true, page, pageSize, search, filters);
                var categoriesTask = ProductCategoryService.GetProductCategoriesByStoreIdAsync(StoreId, StoreConstants.ProductType, true);

                var settings = GetStoreSettings();
                ProductService2.ImageWidth = GetSettingValueInt("ProductsIndex_ImageWidth", 50);
                ProductService2.ImageHeight = GetSettingValueInt("ProductsIndex_ImageHeight", 50);


                await Task.WhenAll(pageDesignTask, productsTask, categoriesTask);
                var products = productsTask.Result;
                var pageDesign = pageDesignTask.Result;
                var categories = categoriesTask.Result;


                

                if (pageDesign == null)
                {
                    throw new Exception("PageDesing is null:" + IndexPageDesingName);
                }


                var pageOutput = ProductService2.GetProductsIndexPage(products, pageDesign, categories);
                var pagingPageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "Paging");


                PagingService2.PageOutput = pageOutput;
                PagingService2.HttpRequestBase = this.Request;
                PagingService2.RouteData = this.RouteData;
                PagingService2.ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                PagingService2.ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                await Task.WhenAll(pagingPageDesignTask);
                var pagingDic = PagingService2.GetPaging(pagingPageDesignTask.Result);
                pagingDic.StoreSettings = settings;
                pagingDic.PageTitle = "Products";
                pagingDic.MyStore = this.MyStore;



                return View(pagingDic);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "ProductsController:Index:" + ex.StackTrace, page);
                return new HttpStatusCodeResult(500);
            }
        }


        public ActionResult Index2()
        {


            return View();
        }

        public async Task<ActionResult> Product(String id = "")
        {

            try
            {

                if (!IsModulActive(StoreConstants.ProductType))
                {
                    Logger.Trace("Navigation Modul is not active:" + StoreConstants.ProductType);
                    return HttpNotFound("Not Found");
                }
                int productId = id.Split("-".ToCharArray()).Last().ToInt();
                var categoryTask = ProductCategoryService.GetProductCategoryAsync(StoreId, productId);
                var productsPageDesignTask = PageDesignService.GetPageDesignByName(StoreId, ProductDetailPage);
                var productsTask = ProductService.GetProductsByIdAsync(productId);


                await Task.WhenAll(productsTask, categoryTask, productsPageDesignTask);
                var product = productsTask.Result;
                var pageDesign = productsPageDesignTask.Result;
                var category = categoryTask.Result;
                var settings = GetStoreSettings();

                ViewData[StoreConstants.MetaTagKeywords] = product.Name;
                ViewData[StoreConstants.MetaTagDescription] = GeneralHelper.TruncateAtWord(GeneralHelper.StripHtml(product.Name + ", " + product.Description), 155);

                if (pageDesign == null)
                {
                    throw new Exception("PageDesing is null:" + ProductDetailPage);
                }
                if (product == null)
                {
                    throw new Exception("Product is NULL. ProductId:" + productId);
                }

                if (category == null)
                {
                    throw new Exception("ProductCategory is NULL.ProductId:" + productId);
                }


                ProductService2.ImageWidth = GetSettingValueInt("ProductsDetail_ImageWidth", 50);
                ProductService2.ImageHeight = GetSettingValueInt("ProductsDetail_ImageHeight", 50);
                var dic = ProductService2.GetProductsDetailPage(product, pageDesign, category);
                dic.MyStore = this.MyStore;
                dic.StoreSettings = settings;
                dic.PageTitle = product.Name;
                dic.MyStore = this.MyStore;
                dic.PageTitle = product.Name;
                return View(dic);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "ProductsController:Product:" + ex.StackTrace);
                return RedirectToAction("Index");
            }
        }

        public ActionResult ProductBuy(int id = 0)
        {
            var productsTask = ProductService.GetProductsById(id);
            return Redirect(productsTask.VideoUrl);
        }




    }
}