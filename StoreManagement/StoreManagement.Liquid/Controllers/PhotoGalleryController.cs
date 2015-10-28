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
    //   [OutputCache(CacheProfile = "Cache20Minutes")]
    public class PhotoGalleryController : BaseController
    {
        private const String IndexPageDesingName = "PhotoGalleryIndexPage";

        public async Task<ActionResult> Index(int page = 1)
        {
            try
            {

                var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, IndexPageDesingName);
                var pageSize = GetSettingValueInt("PhotoGallery_PageSize", StoreConstants.DefaultPageSize);
                var fileManagersTask = FileManagerService.GetImagesByFileSizeAsync(StoreId, "ShopStyle", "Best,Large", page, pageSize);
                var settings = GetStoreSettings();
                PhotoGalleryHelper.StoreSettings = settings;
                PhotoGalleryHelper.ImageWidth = GetSettingValueInt("PhotoGallery_ImageWidth", 500);
                PhotoGalleryHelper.ImageHeight = GetSettingValueInt("PhotoGallery_ImageHeight", 500);

                await Task.WhenAll(pageDesignTask, fileManagersTask);
                var pageDesign = pageDesignTask.Result;

                if (pageDesign == null)
                {
                    throw new Exception("PageDesing is null:" + IndexPageDesingName);
                }


                var fileManagers = fileManagersTask.Result;

                var pageOutput = PhotoGalleryHelper.GetPhotoGalleryIndexPage(pageDesign, fileManagers);

                var pagingPageDesignTask = PageDesignService.GetPageDesignByName(StoreId, "Paging");


                PagingHelper.StoreSettings = settings;
                PagingHelper.StoreId = StoreId;
                PagingHelper.PageOutput = pageOutput;
                PagingHelper.HttpRequestBase = this.Request;
                PagingHelper.RouteData = this.RouteData;
                PagingHelper.ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                PagingHelper.ControllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                await Task.WhenAll(pagingPageDesignTask);
                var pagingDic = PagingHelper.GetPaging(pagingPageDesignTask.Result);


                return View(pagingDic);

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "PhotoGallery:Index:" + ex.StackTrace);
                return new HttpStatusCodeResult(500);
            }
        }


    }
}