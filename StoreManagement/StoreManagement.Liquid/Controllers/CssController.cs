using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StoreManagement.Liquid.Controllers
{
    public class CssController : BaseController
    {
        private const String CssFilePageDesingName = "SiteCssFile";
        public async Task<ContentResult> GetTheme()
        {

            var pageDesignTask = PageDesignService.GetPageDesignByName(StoreId, CssFilePageDesingName);

            await Task.WhenAll(pageDesignTask);
            var pageDesign = pageDesignTask.Result;

            if (pageDesign == null)
            {
                throw new Exception("PageDesing is null:" + CssFilePageDesingName);
            }


            return Content(pageDesign.PageTemplate, "text/css");
        }
	}
}