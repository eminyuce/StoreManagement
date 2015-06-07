using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using StoreManagement.Data.Entities;
using StoreManagement.Data.RequestModel;

namespace StoreManagement.Controllers
{
    public class BlogsController : BaseController
    {
        //
        // GET: /Blogs/
        public ActionResult Index(int page)
        {
            int categoryId = 1;
            var newsContents = new ContentsViewModel();
            newsContents.Store = Store;
            var m = ContentService.GetContentsCategoryId(Store.Id, categoryId, "blog", true, page, 24);
            newsContents.Contents = new PagedList<Content>(m.items, m.page - 1, m.pageSize, m.totalItemCount);
            return View(newsContents);
        }
	}
}