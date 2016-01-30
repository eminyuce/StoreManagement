using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using NLog;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;


namespace StoreManagement.Controllers
{
    [OutputCache(CacheProfile = "Cache1Days")]
    public class NewsController : BaseController
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const String ContentType = StoreConstants.NewsType;

        public ActionResult Index(int page = 1)
        {
            if (!IsModulActive(ContentType))
            {
                return HttpNotFound("Not Found");
            }

            ContentsViewModel resultModel = ContentService2.GetContentIndexPage(page, ContentType);
            return View(resultModel);
        }
        public ActionResult Detail(String id)
        {
            if (!IsModulActive(ContentType))
            {
                return HttpNotFound("Not Found");
            }

            ContentDetailViewModel resultModel = ContentService2.GetContentDetail(id, ContentType);
            return View(resultModel);
        }
    }
}