using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace StoreManagement.Controllers
{
    [OutputCache(CacheProfile = "Cache1Days")]
    public class PagesController : BaseController
    {

         

        public ActionResult Index()
        {
            return View();
        }
	}
}