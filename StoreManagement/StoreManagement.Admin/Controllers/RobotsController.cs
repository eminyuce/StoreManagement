using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data;

namespace StoreManagement.Admin.Controllers
{

    public class RobotsController : Controller
    {
        public FileContentResult RobotsText()
        {
            var content = "User-agent: *" + Environment.NewLine;
            String siteStatus = ProjectAppSettings.GetWebConfigString("SiteStatus", "dev");
            

          //  if (string.Equals(siteStatus, "live", StringComparison.InvariantCultureIgnoreCase))
            if (siteStatus.IndexOf("live", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                content += "Disallow: " + Environment.NewLine;

            }
            else
            {
                content += "Disallow: /" + Environment.NewLine;
                //content += "Disallow: /" + Environment.NewLine;

            }

            // content += siteStatus;

            return File(Encoding.UTF8.GetBytes(content), "text/plain");
        }
    }


}