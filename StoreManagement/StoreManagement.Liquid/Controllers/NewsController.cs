using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data;
using StoreManagement.Data.Constants;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Liquid.Helper;
using StoreManagement.Liquid.Helper.Interfaces;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Liquid.Controllers
{
    public class NewsController : ContentsController
    {
        public NewsController()
            : base(StoreConstants.NewsType)
        {
            this.PageDesingDetailPageName = "NewsDetailPage";
            this.PageDesingIndexPageName = "NewsIndexPage";
        }
        
    }
}