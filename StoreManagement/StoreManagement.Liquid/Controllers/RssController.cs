using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Liquid.Controllers
{
    public class RssController : BaseController
    {
      

        public ActionResult Products()
        {
            return View();
        }
        public ActionResult News()
        {
            return View();
        }
        public ActionResult Blogs()
        {
            return View();
        }

       
    }
}