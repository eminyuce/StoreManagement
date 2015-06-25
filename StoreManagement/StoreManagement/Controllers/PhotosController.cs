using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using StoreManagement.Data.Entities;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Controllers
{
    public class PhotosController : BaseController
    {
         

        public ActionResult Index(int page = 1)
        {
            var photos = new PhotosViewModel();
            photos.Store = this.Store;
            var m = FileManagerService.GetImagesByStoreId(Store.Id, page, 24);
            photos.FileManagers = new PagedList<FileManager>(m.items, m.page - 1, m.pageSize, m.totalItemCount);
            return View(photos);
        }
	}
}