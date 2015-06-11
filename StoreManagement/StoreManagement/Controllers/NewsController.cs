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
    public class NewsController : BaseController
    {
        //
        // GET: /News/
        public ActionResult Index(int page=1)
        {
            var newsContents = new ContentsViewModel();
            newsContents.Store = Store;
            var m = ContentService.GetContentsCategoryId(Store.Id, null, "news", true, page, 24);
            newsContents.Contents = new PagedList<Content>(m.items, m.page - 1, m.pageSize, m.totalItemCount);
            return View(newsContents);
        }
	}
}