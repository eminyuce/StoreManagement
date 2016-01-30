using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using NLog;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.DbContext;

using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Controllers
{
    [OutputCache(CacheProfile = "Cache1Days")]
    public abstract class ContentsController : BaseController
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private String ContentType { get; set; }

        protected ContentsController(String contentType)
        {
            this.ContentType = contentType;
        }

        public virtual ActionResult Index(int page = 1)
        {
            if (!IsModulActive(ContentType))
            {
                return HttpNotFound("Not Found");
            }

            ContentsViewModel resultModel = ContentService2.GetContentIndexPage(page, ContentType);
            return View(resultModel);
        }
        public virtual ActionResult Detail(String id)
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