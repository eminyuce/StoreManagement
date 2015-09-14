using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StoreManagement.Liquid.Controllers
{
    public class ContactsController : BaseController
    {
       
        [OutputCache(CacheProfile = "Cache20Minutes")]
        public ActionResult Index()
        {
            try
            {

                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "ContactsIndex");

                ContactHelper.StoreSettings = GetStoreSettings();
                ContactHelper.ImageWidth = GetSettingValueInt("ContactsIndex_ImageWidth", 50);
                ContactHelper.ImageHeight = GetSettingValueInt("ContactsIndex_ImageHeight", 50);
                var contactsTask = ContactService.GetContactsByStoreIdAsync(StoreId, null, true);
                var pageOutput = ContactHelper.GetContactIndexPage(pageDesignTask, contactsTask);


                return View(pageOutput);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Index:" + ex.StackTrace);
                return new HttpStatusCodeResult(500);
            }
        }
	}
}