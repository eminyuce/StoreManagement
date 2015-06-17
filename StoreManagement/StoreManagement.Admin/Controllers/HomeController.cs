using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc;
using System.Web.Security;
using StoreManagement.Data.Entities;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;
using StoreManagement.Data.GeneralHelper;


namespace StoreManagement.Admin.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Dashboard");
        }

      
        public ActionResult NoAccessPage(int id)
        {
            int storeId = id;
            Logger.Info("NoAccessPage. StoreId:"+storeId);
            return View();
        }

         //<li>
         //                           <a href="@url">Go to frontend <i class="glyphicon glyphicon-share-alt"></i></a>
         //                       </li>

        public ActionResult ReturnFrontEndUrl()
        {
            if (IsSuperAdmin)
            {
                return new EmptyResult();
            }
            else
            {
                return PartialView("ReturnFrontEndUrl", this.LoginStore);
            }
              
        }
    }
}
