using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            private const String IndexPageDesingName = "PhotoGalleryIndexPage";

        public async Task<ActionResult> Index()
        {
            try
            {

                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, IndexPageDesingName);
                var fileManagersTask = FileManagerService.GetImagesByStoreIdAsync(StoreId, true);

                PhotoGalleryHelper.StoreSettings = GetStoreSettings();
                PhotoGalleryHelper.ImageWidth = GetSettingValueInt("PhotoGallery_ImageWidth", 500);
                PhotoGalleryHelper.ImageHeight = GetSettingValueInt("PhotoGallery_ImageHeight", 500);

                await Task.WhenAll(pageDesignTask, fileManagersTask);
                var pageDesign = pageDesignTask.Result;

                if (pageDesign == null)
                {
                    throw new Exception("PageDesing is null:" + IndexPageDesingName);
                }


                var fileManagers = fileManagersTask.Result;

                var dic = PhotoGalleryHelper.GetPhotoGalleryIndexPage(pageDesign, fileManagers);

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