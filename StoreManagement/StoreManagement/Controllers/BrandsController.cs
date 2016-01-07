using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;

namespace StoreManagement.Controllers
{
    [OutputCache(CacheProfile = "Cache1Days")]
    public class BrandsController : BaseController
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        // GET: /Brands/
        public ActionResult Index()
        {
            return View();
        }
	}
}