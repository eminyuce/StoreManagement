using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using NLog;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;


namespace StoreManagement.Controllers
{
    [OutputCache(CacheProfile = "Cache1Days")]
    public class ProductsController : BaseController
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index(String search="", String page="1")
        {
           
            ProductsViewModel resultModel = ProductService2.GetProductIndexPage(search, page);
            return View(resultModel);
        }
        //
        // GET: /Products/
        public ActionResult Product(String id)
        {
            
            ProductDetailViewModel resultModel = ProductService2.GetProductDetailPage(id);
            return View(resultModel);
        }
	}
}