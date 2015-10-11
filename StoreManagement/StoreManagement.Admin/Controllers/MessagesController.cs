using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StoreManagement.Admin.Controllers
{
    public class MessagesController : BaseController
    {
        //
        // GET: /Messages/
        public ActionResult Index()
        {
            return View();
        }
	}
}