using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;

namespace StoreManagement.Data.GeneralHelper
{
    public class PartialViewToString
    {
        public string RenderPartialToString(ControllerContext context,
           string partialViewName, ViewDataDictionary viewData, TempDataDictionary tempData)
        {
             
            if (tempData == null)
            {
                tempData  = new TempDataDictionary();
            }

            ViewEngineResult result = ViewEngines.Engines.FindPartialView(context, partialViewName);

            if (result.View != null)
            {
                StringBuilder sb = new StringBuilder();
                using (StringWriter sw = new StringWriter(sb))
                {
                    using (HtmlTextWriter output = new HtmlTextWriter(sw))
                    {
                        ViewContext viewContext = new ViewContext(context, result.View, viewData, tempData, output);
                        //  viewContext.ViewBag.location = location;
                        result.View.Render(viewContext, output);
                    }
                }

                return sb.ToString();
            }

            return String.Empty;
        }

    }
}
