using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Controllers
{
    public class ContentsController : BaseController
    {


        //
        // GET: /Contents/
        public ContentsController(IStoreContext dbContext, ISettingRepository settingRepository, IStoreRepository storeRepository) : base(dbContext, settingRepository, storeRepository)
        {

        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Detail(String id)
        {
            int contentId = id.Split("-".ToCharArray()).Last().ToInt();
            var contentDetail = new ContentDetailViewModel();
            contentDetail.Content = ContentRepository.GetAllIncluding(r2 => r2.ContentFiles.Select(r3 => r3.FileManager))
                                 .FirstOrDefault(r1 => r1.Id == contentId);
            return View(contentDetail);
        }
	}
}