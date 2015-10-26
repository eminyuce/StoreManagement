using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data;
using StoreManagement.Data.Constants;

namespace StoreManagement.Liquid.Controllers
{
    public class RobotsController : BaseController
    {
        /// <summary>
        /// Tells search engines (or robots) how to index your site. 
        /// The reason for dynamically generating this code is to enable generation of the full absolute sitemap URL
        /// and also to give you added flexibility in case you want to disallow search engines from certain paths.
        /// The sitemap is cached for one day, adjust this time to whatever you require.
        /// See <see cref="http://en.wikipedia.org/wiki/Robots_exclusion_standard"/> for more information.
        /// </summary>
        /// <returns>The robots text for the current site.</returns>
        public FileContentResult RobotsText()
        {
            var defaultContent = "User-agent: *" + Environment.NewLine;
            defaultContent += "Disallow: /" + Environment.NewLine;
            var content = GetSettingValue(StoreConstants.RobotsTxt, defaultContent);
            return File(Encoding.UTF8.GetBytes(defaultContent), "text/plain");
        }
	}
}