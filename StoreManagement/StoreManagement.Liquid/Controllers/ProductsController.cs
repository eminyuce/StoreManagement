using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Constants;
using StoreManagement.Data.EntitiesWrapper;
using StoreManagement.Liquid.Helper;

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

                var productsPageDesignTask = PageDesignService.GetPageDesignByName(Store.Id, "ProductsIndex");
                var productsTask = ProductService.GetProductsCategoryIdAsync(Store.Id, null, StoreConstants.ProductType, true, page, GetSettingValueInt("ProductsIndexPageSize", StoreConstants.DefaultPageSize));
                var categories = ProductCategoryService.GetProductCategoriesByStoreIdAsync(Store.Id, StoreConstants.ProductType, true);
                var dic = ProductHelper.GetProductsIndexPage(this.HttpContext.Request, productsTask, productsPageDesignTask, categories);

                return View(dic);

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

        public ActionResult Product()
        {
            return View();
        }
        public ActionResult Product2()
        {



            return View();
        }

    }
}