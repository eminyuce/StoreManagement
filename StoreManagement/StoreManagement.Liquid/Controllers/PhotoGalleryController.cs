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
        [OutputCache(CacheProfile = "Cache20Minutes")]
    public class PhotoGalleryController : BaseController
    {
        

        public ActionResult Index()
        {
            try
            {

                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "PhotoGalleryIndex");
                var fileManagers = FileManagerService.GetImagesByStoreIdAsync(StoreId, true);

                PhotoGalleryHelper.StoreSettings = GetStoreSettings();
                PhotoGalleryHelper.ImageWidth = GetSettingValueInt("PhotoGallery_ImageWidth", 500);
                PhotoGalleryHelper.ImageHeight = GetSettingValueInt("PhotoGallery_ImageHeight", 500);
                var dic = PhotoGalleryHelper.GetPhotoGalleryIndexPage(pageDesignTask, fileManagers);

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