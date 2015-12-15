using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Controllers
{
    public class CssController : BaseController
    {
         

        public ContentResult GetTheme()
        {
            var builder = new StringBuilder();
            //IDictionary<string, IDictionary<string, string>> css = new Dictionary<string, IDictionary<string, string>>();

            var settingStyle = this.SettingService.GetStoreSettings(MyStore.Id)
                .Where(r => r.Type.ToLower().Contains("Style".ToLower())).ToList();

            /* Populate css object from the database */

            foreach (var selector in settingStyle)
            {
                builder.Append(selector.SettingKey);
                builder.AppendLine(" { ");
                builder.AppendLine(selector.SettingValue);
                builder.AppendLine("}");
            }

            return Content(builder.ToString(), "text/css");
        }
    }
}