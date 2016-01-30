using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data;

namespace StoreManagement.Controllers
{
    public class RobotsController : Controller
    {
        public FileContentResult RobotsText()
        {
            var content = "User-agent: *" + Environment.NewLine;
            String siteStatus = ProjectAppSettings.GetWebConfigString("SiteStatus", "dev");
            content += "Disallow: /" + Environment.NewLine;
            return File(Encoding.UTF8.GetBytes(content), "text/plain");
        }
	}
}