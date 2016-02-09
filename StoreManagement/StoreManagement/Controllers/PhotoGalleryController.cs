using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using NLog;
using StoreManagement.Data.Entities;
using StoreManagement.Data.RequestModel;


namespace StoreManagement.Controllers
{
    [OutputCache(CacheProfile = "Cache1Days")]
    public class PhotoGalleryController : BaseController
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index(int page = 1)
        {

            var photos = FileManagerService.GetPhotos(page);
            return View(photos);
        }
	}
}