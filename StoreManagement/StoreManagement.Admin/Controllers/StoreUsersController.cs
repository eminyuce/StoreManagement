using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StoreManagement.Admin.Controllers
{

    [Authorize]
    public class StoreUsersController : UsersController
    {
        public ActionResult Index(  int storeId = 0,String search="")
        {
            storeId = GetStoreId(storeId);
            return this.Users(storeId, search);
        }
	}
}