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
    public class NewsController : ContentsController
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const String ContentType = StoreConstants.NewsType;


        public NewsController(string contentType)
            : base(ContentType)
        {

        }
    }
}