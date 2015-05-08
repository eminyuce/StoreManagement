using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StoreManagement.Admin.Extensions
{
    public static class LabelExtensions
    {
        public static string SettingsLabel(this HtmlHelper helper, string settings)
        {
            return String.Format("<label for='settings'>{0}</label>", settings);

        }
    }
}