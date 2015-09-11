using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Liquid.Helper.Interfaces
{
    public interface IPagingHelper : IHelper
    {
        StoreLiquidResult PageOutput { get; set; }
        string ActionName { get; set; }
        string ControllerName { get; set; }
        HttpRequestBase HttpRequestBase { get; set; }
        RouteData RouteData { get; set; }
        StoreLiquidResult GetPaging(Task<PageDesign> pagingPageDesignTask);
    }

}