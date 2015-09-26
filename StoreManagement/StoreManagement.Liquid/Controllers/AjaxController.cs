using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.CacheHelper;

namespace StoreManagement.Liquid.Controllers
{
    public abstract class AjaxController : BaseController
    {
        protected static readonly TypedObjectCache<String> AjaxRequestCache = new TypedObjectCache<String>("AjaxRequestCache");
	}
}