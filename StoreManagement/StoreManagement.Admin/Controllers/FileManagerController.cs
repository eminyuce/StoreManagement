using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Service.DbContext;

namespace StoreManagement.Admin.Controllers
{
    public class FileManagerController : BaseController
    {
        public FileManagerController(IStoreContext dbContext) : base(dbContext)
        {

        }
        public ActionResult Index()
        {
            return View();
        }
    }
}