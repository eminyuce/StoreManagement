using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.Entities;

namespace StoreManagement.Data.GeneralHelper
{
    public static class PartialViewToString
    {

        private static readonly TypedObjectCache<String> PartialViewToStringCache = new TypedObjectCache<String>("PartialViewToString");
        public static string RenderPartialToString(this Controller controller, string partialViewName, ViewDataDictionary viewData, TempDataDictionary tempData)
        {
            ControllerContext controllerContext = controller.ControllerContext;
            if (tempData == null)
            {
                tempData = new TempDataDictionary();
            }

            ViewEngineResult result = ViewEngines.Engines.FindPartialView(controllerContext, partialViewName);

            if (result.View != null)
            {
                StringBuilder sb = new StringBuilder();
                using (StringWriter sw = new StringWriter(sb))
                {
                    using (HtmlTextWriter output = new HtmlTextWriter(sw))
                    {
                        ViewContext viewContext = new ViewContext(controllerContext, result.View, viewData, tempData, output);
                        //  viewContext.ViewBag.location = location;
                        result.View.Render(viewContext, output);
                    }
                }

                return sb.ToString();
            }

            return String.Empty;
        }
        public static string RenderPartialToString(this Controller controller, string partialView, ViewDataDictionary viewData)
        {
            return RenderPartialToString(controller, partialView, viewData, new TempDataDictionary());
        }
        public static string RenderPartialToStringCache(this Controller controller, string partialView, ViewDataDictionary viewData)
        {
            
            String key = String.Format("RenderPartialToStringCache-{0}", partialView);
            String item = null;
            PartialViewToStringCache.TryGet(key, out item);

            if (String.IsNullOrEmpty(item))
            {
                item = RenderPartialToString(controller, partialView, viewData, new TempDataDictionary());
                PartialViewToStringCache.Set(key, item, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.CacheLongSeconds));
            }

            return item;
        }
        public static string RenderPartialToString(this Controller controller, string partialView)
        {
            return RenderPartialToString(controller, partialView, new ViewDataDictionary(), new TempDataDictionary());
        }

        public static string RenderPartialToString(this Controller controller, string partialView, object model)
        {
            return RenderPartialToString(controller, partialView, new ViewDataDictionary(model), new TempDataDictionary());
        }

    }
}
