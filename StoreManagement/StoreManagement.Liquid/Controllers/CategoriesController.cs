using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StoreManagement.Liquid.Controllers
{
    public class CategoriesController : BaseController
    {
        //
        // GET: /Categories/
        public ActionResult Index()
        {
            return View();
        }
	}
}