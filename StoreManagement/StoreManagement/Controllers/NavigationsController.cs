using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Controllers
{
    public class NavigationsController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }
	}
}