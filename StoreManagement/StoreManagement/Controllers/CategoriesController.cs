using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using NLog;
using Ninject;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.DbContext;

using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Controllers
{
    [OutputCache(CacheProfile = "Cache1Days")]
    public abstract class CategoriesController : BaseController
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private String ContentType { get; set; }

        protected CategoriesController(String contentType)
        {
            this.ContentType = contentType;
        }
       
        public virtual ActionResult Category(String id)
        {
            if (!IsModulActive(ContentType))
            {
                return HttpNotFound("Not Found");
            }

            CategoryViewModel resultModel = CategoryService2.GetCategory(id, 24, ContentType);
            return View(resultModel);
        }
    }
}