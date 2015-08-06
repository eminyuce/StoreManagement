using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Constants;
using StoreManagement.Liquid.Helper;

namespace StoreManagement.Liquid.Controllers
{
    public class PhotoGalleryController : BaseController
    {
        //
        // GET: /PhotoGallery/
        public ActionResult Index()
        {
            try
            {

                var pageDesignTask = PageDesignService.GetPageDesignByName(Store.Id, "PhotoGalleryIndex");
                var fileManagers = FileManagerService.GetImagesByStoreIdAsync(Store.Id, true);
                var liquidHelper = new PhotoGalleryHelper();
                liquidHelper.StoreSettings = StoreSettings;
                var dic = liquidHelper.GetPhotoGalleryIndexPage(pageDesignTask, fileManagers);

                return View(dic);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "PhotoGallery:Index:" + ex.StackTrace);
                return new HttpStatusCodeResult(500);
            }
        }
	}
}