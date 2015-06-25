using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Controllers
{
    public class ContentsController : BaseController
    {
         

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Detail(String id)
        {
            int contentId = id.Split("-".ToCharArray()).Last().ToInt();
            var contentDetail = new ContentDetailViewModel();
            contentDetail.Content = ContentService.GetContentWithFiles(contentId);


            return View(contentDetail);
        }
	}
}