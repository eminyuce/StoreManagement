using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;
using StoreManagement.Data.EntitiesWrapper;

namespace StoreManagement.Data.LiquidEngineHelpers
{
    public class LiquidEngineHelper
    {


        public static String RenderPage(string templateCode, object anonymousObject)
        {
            Template template = Template.Parse(templateCode);
            return template.Render(Hash.FromAnonymousObject(anonymousObject));
        }
    }
}
