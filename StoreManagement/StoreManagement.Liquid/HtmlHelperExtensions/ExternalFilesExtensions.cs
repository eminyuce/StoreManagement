using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Ninject;
using StoreManagement.Data.GeneralHelper;

namespace StoreManagement.Liquid.HtmlHelperExtensions
{
    public static class ExternalFilesExtensions
    {
     //   [Inject]
      //  public IPageDesignService PageDesignService { set; get; }

        public static MvcHtmlString MenuItem(
         this HtmlHelper htmlHelper,
         string text,
         string action,
         string controller,
         string liCssClass = null
     )
        {
            var li = new TagBuilder("li");
            if (!String.IsNullOrEmpty(liCssClass))
            {
                li.AddCssClass(liCssClass);
            }
            var routeData = htmlHelper.ViewContext.RouteData;
            var currentAction = routeData.GetRequiredString("action");
            var currentController = routeData.GetRequiredString("controller");
            if (string.Equals(currentAction, action, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(currentController, controller, StringComparison.OrdinalIgnoreCase))
            {
                li.AddCssClass("active");
            }
            li.InnerHtml = String.Format("<a href=\"{0}\"><i class=\"\"></i>{1}</a>",
               new UrlHelper(htmlHelper.ViewContext.RequestContext).Action(action, controller).ToString()
                , text);
            return MvcHtmlString.Create(li.ToString());
        }
        //public static  MvcHtmlString MainLayoutJavaScriptFiles(this HtmlHelper htmlHelper, int storeId)
        //{
        //    var pageDesignTask = PageDesignService.GetPageDesignByName(storeId, "MainLayoutJavaScriptFiles");
        //    Task.WaitAll(pageDesignTask);
        //    var result = pageDesignTask.Result.PageTemplate.HtmlDecode();
        //    return MvcHtmlString.Create(result);

        //}
        //public static MvcHtmlString MainLayoutCssFiles(this HtmlHelper htmlHelper, int storeId)
        //{
        //    var pageDesignTask = PageDesignService.GetPageDesignByName(storeId, "MainLayoutCssFiles");
        //    Task.WaitAll(pageDesignTask);
        //    var result = pageDesignTask.Result.PageTemplate.HtmlDecode();
        //    return MvcHtmlString.Create(result);

        //}

    }
}