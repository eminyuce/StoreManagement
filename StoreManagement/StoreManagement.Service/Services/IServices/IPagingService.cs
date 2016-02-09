using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.LiquidHelpers.Interfaces;

namespace StoreManagement.Service.Services.IServices
{
    public interface IPagingService : IBaseService
    {
        StoreLiquidResult PageOutput { get; set; }
        string ActionName { get; set; }
        string ControllerName { get; set; }
        HttpRequestBase HttpRequestBase { get; set; }
        RouteData RouteData { get; set; }
        StoreLiquidResult GetPaging(PageDesign pagingPageDesignTask);
    }
}
