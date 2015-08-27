using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Constants;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Liquid.Helper;

namespace StoreManagement.Liquid.Controllers
{
    public class BrandsController : BaseController
    {
        //
        // GET: /Brands/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(String id = "")
        {
            try
            {

                int brandId = id.Split("-".ToCharArray()).Last().ToInt();
                var blogsPageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "BrandDetailPage");
                var brandTask = BrandService.GetBrandAsync(brandId);
                var take = GetSettingValueInt("BrandProducts_ItemNumber", 20);
                var productsTask = ProductService.GetProductsByBrandAsync(StoreId, brandId, take, 0);
                var productCategoriesTask = ProductCategoryService.GetCategoriesByBrandIdAsync(StoreId, brandId);
                var liquidHelper = new BrandHelper();
                liquidHelper.StoreSettings = GetStoreSettings();
                liquidHelper.ImageWidth = GetSettingValueInt("BrandDetail_ImageWidth", 50);
                liquidHelper.ImageHeight = GetSettingValueInt("BrandDetail_ImageHeight", 50);
                var dic = liquidHelper.GetBrandDetailPage(brandTask, productsTask, blogsPageDesignTask, productCategoriesTask);


                return View(dic);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "NewsController:News:" + ex.StackTrace);
                return new HttpStatusCodeResult(500);
            }
        }
    }
}