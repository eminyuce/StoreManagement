using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StoreManagement.Liquid.Controllers
{
    public class ContactsController : BaseController
    {
        //
        // GET: /Contacts/
        public ActionResult Index()
        {
            try
            {

                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "ContactsIndex");

                ContactHelper.StoreSettings = GetStoreSettings();
                ContactHelper.ImageWidth = GetSettingValueInt("ContactsIndex_ImageWidth", 50);
                ContactHelper.ImageHeight = GetSettingValueInt("ContactsIndex_ImageHeight", 50);

                var pageOutput = LocationHelper.GetLocationIndexPage(pageDesignTask);


                return View(pageOutput);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "ProductsController:Index:" + ex.StackTrace);
                return new HttpStatusCodeResult(500);
            }
        }
	}
}