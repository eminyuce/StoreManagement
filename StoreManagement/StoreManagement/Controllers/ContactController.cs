using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StoreManagement.Controllers
{
    public class ContactController : BaseController
    {
        //
        // GET: /Contact/
        public ActionResult Index()
        {
            return View();
        }
	}
}