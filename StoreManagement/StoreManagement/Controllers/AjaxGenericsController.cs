using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using StoreManagement.Data.Attributes;

namespace StoreManagement.Controllers
{
    [OutputCache(CacheProfile = "Cache1Days")]
    [Compress]
    public class AjaxGenericsController : AjaxController
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        //
        // GET: /AjaxGenerics/
        public ActionResult Index()
        {
            return View();
        }
	}
}