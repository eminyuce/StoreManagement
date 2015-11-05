using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;
using NLog;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidFilters;

namespace StoreManagement.Data.LiquidEngineHelpers
{
    public class LiquidEngineHelper
    {

        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static String RenderPage(PageDesign pageDesign, object anonymousObject)
        {
            String renderHtml = "";
            String templateCode = pageDesign.PageTemplate;
            String pageDesignName = pageDesign.Name;
            try
            {


                Template.RegisterFilter(typeof(TextFilter));
                Template template = Template.Parse(templateCode);

                String renderPage = template.Render(Hash.FromAnonymousObject(anonymousObject));
                if (template.Errors.Any())
                {
                    foreach (var e in template.Errors)
                    {
                        Logger.Error(" Template Rending Errors:" + e.StackTrace + " templateCode:" + templateCode + " PAGE DESING NAME:" + pageDesignName);
                        Logger.Error(e, "Template Rending Errors:" + e.StackTrace + " templateCode:" + templateCode, anonymousObject + " PAGE DESING NAME:" + pageDesignName);
                    }
                }

                renderHtml = renderPage;

            }
            catch (Exception ex)
            {
                Logger.Error("RenderPage:" + ex.StackTrace + " templateCode:" + templateCode + " PAGE DESING NAME:" + pageDesignName);
                renderHtml = "ERROR";
            }

            return renderHtml;
        }
    }
}
