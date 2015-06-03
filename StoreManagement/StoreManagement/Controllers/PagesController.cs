using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StoreManagement.Controllers
{
    public class PagesController : BaseController
    {
        //
        // GET: /Pages/
        public ActionResult Index()
        {
            return View();
        }
	}
}