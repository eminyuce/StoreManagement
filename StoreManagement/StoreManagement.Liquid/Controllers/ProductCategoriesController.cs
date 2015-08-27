using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Constants;
using StoreManagement.Liquid.Helper;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Liquid.Controllers
{
    public class ProductCategoriesController : BaseController
    {
        private const String PageDesingName = "ProductCategoriesIndex";
        

        public ActionResult Index(int page = 1)
        {
            try
            {
                if (!IsModulActive(StoreConstants.ProductType))
                {
                    return HttpNotFound("Not Found");
                }

                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, PageDesingName);
                var pageSize = GetSettingValueInt(PageDesingName, StoreConstants.DefaultPageSize);
                var categories = ProductCategoryService.GetProductCategoriesByStoreIdAsync(StoreId, StoreConstants.ProductType, true, page, pageSize);

                ProductCategoryHelper.StoreSettings = GetStoreSettings();
                var dic = ProductCategoryHelper.GetCategoriesIndexPage(pageDesignTask, categories);

                return View(dic);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "ProductCategories:Index:" + ex.StackTrace, page);
                return new HttpStatusCodeResult(500);
            }
        }


   //
        // GET: /Product/
        public ActionResult Category(String id="")
        {
            if (!IsModulActive(StoreConstants.ProductType))
            {
                return HttpNotFound("Not Found");
            }


            return View();
        }


       
    }
}