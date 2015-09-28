using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StoreManagement.Liquid.Controllers
{
    public class ContactsController : BaseController
    {
       
        [OutputCache(CacheProfile = "Cache20Minutes")]
        public async Task<ActionResult> Index()
        {
            try
            {

                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "ContactsIndexPage");

                ContactHelper.StoreSettings = GetStoreSettings();
                ContactHelper.ImageWidth = GetSettingValueInt("ContactsIndex_ImageWidth", 50);
                ContactHelper.ImageHeight = GetSettingValueInt("ContactsIndex_ImageHeight", 50);
                var contactsTask = ContactService.GetContactsByStoreIdAsync(StoreId, null, true);

                await Task.WhenAll(pageDesignTask, contactsTask);
                var pageDesign = pageDesignTask.Result;
                var contacts = contactsTask.Result;

                if (pageDesign == null)
                {
                    throw new Exception("PageDesing is null");
                }


                var pageOutput = ContactHelper.GetContactIndexPage(pageDesign, contacts);


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