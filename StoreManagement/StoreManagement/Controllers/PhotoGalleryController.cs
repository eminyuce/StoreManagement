using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using NLog;
using StoreManagement.Data.Entities;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Controllers
{
    [OutputCache(CacheProfile = "Cache1Days")]
    public class PhotoGalleryController : BaseController
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index(int page = 1)
        {
            var photos = new PhotosViewModel();
            photos.Store = this.MyStore;
            var m = FileManagerService.GetImagesByStoreId(MyStore.Id, page, 24);
            photos.FileManagers = new PagedList<FileManager>(m.items, m.page - 1, m.pageSize, m.totalItemCount);
            return View(photos);
        }
	}
}