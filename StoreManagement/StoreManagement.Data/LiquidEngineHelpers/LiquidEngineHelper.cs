using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;
using NLog;
using StoreManagement.Data.LiquidFilters;

namespace StoreManagement.Data.LiquidEngineHelpers
{
    public class LiquidEngineHelper
    {

        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static String RenderPage(string templateCode, object anonymousObject)
        {
            Template.RegisterFilter(typeof(TextFilter));
            Template template = Template.Parse(templateCode);
       
            String renderPage =  template.Render(Hash.FromAnonymousObject(anonymousObject));
            if (template.Errors.Any())
            {
                foreach (var e in template.Errors)
                {
                    Logger.Error(e, "Template Rending Errors:" + e.StackTrace, templateCode, anonymousObject);
                }
            }
             
            return renderPage;
        }
    }
}
