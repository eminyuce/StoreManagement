using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Constants;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Liquid.Helper;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Liquid.Controllers
{
    public class ProductsController : BaseController
    {
       

        public ActionResult Index(int page = 1)
        {
            try
            {
                if (!IsModulActive(StoreConstants.ProductType))
                {
                    return HttpNotFound("Not Found");
                }
                var pagingPageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "Paging");
                var productsPageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "ProductsIndex");
                var productsTask = ProductService.GetProductsCategoryIdAsync(StoreId, null, StoreConstants.ProductType, true, page, GetSettingValueInt("ProductsIndex_PageSize", StoreConstants.DefaultPageSize));
                var categories = ProductCategoryService.GetProductCategoriesByStoreIdAsync(StoreId, StoreConstants.ProductType, true);


                ProductHelper.StoreSettings = GetStoreSettings();
                ProductHelper.ImageWidth = GetSettingValueInt("ProductsIndex_ImageWidth", 50);
                ProductHelper.ImageHeight = GetSettingValueInt("ProductsIndex_ImageHeight", 50);

                var pageOutput = ProductHelper.GetProductsIndexPage(productsTask, productsPageDesignTask, categories);


                var pagingHelper = new PagingHelper();
                pagingHelper.StoreSettings = GetStoreSettings();
                pagingHelper.StoreId = StoreId;
                pagingHelper.PageOutput = pageOutput;
                pagingHelper.ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                pagingHelper.ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                var pagingDic = pagingHelper.GetPaging(pagingPageDesignTask);


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

        public ActionResult Product(String id = "")
        {

            try
            {

                if (!IsModulActive(StoreConstants.ProductType))
                {
                    return HttpNotFound("Not Found");
                }
                int productId = id.Split("-".ToCharArray()).Last().ToInt();
                var categoryTask = ProductCategoryService.GetProductCategoryAsync(StoreId, productId);
                var productsPageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "ProductDetailPage");
                var productsTask = ProductService.GetProductsByIdAsync(productId);

                ProductHelper.ImageWidth = GetSettingValueInt("ProductsDetail_ImageWidth", 50);
                ProductHelper.ImageHeight = GetSettingValueInt("ProductsDetail_ImageHeight", 50);
                ProductHelper.StoreSettings = GetStoreSettings();
                var dic = ProductHelper.GetProductsDetailPage(productsTask, productsPageDesignTask, categoryTask);

                return View(dic);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "ProductsController:Product:" + ex.StackTrace);
                return new HttpStatusCodeResult(500);
            }
        }
        public ActionResult Product2()
        {



            return View();
        }
 
    }
}