using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;

namespace StoreManagement.Data.LiquidEntities
{
    public class StoreLiquidResult
    {
        public Store MyStore { get; set; }
        public String PageTitle { get; set; }
        public Dictionary<String, String> LiquidRenderedResult { get; set; }

        public String PageOutputText
        {
            get
            {
                return LiquidRenderedResult[StoreConstants.PageOutput];
            }
        }
        public String PageDesingName { get; set; }
        public string DetailLink { get; set; }
        public string GetCanonicalUrl(HttpContextBase httpRequestBase)
        {
            HttpRequestBase request = httpRequestBase.Request;
            var domainName = GeneralHelper.GeneralHelper.GetSiteDomain(httpRequestBase);
            if (request.Url != null)
            {
                var m = request.Url.Scheme + Uri.SchemeDelimiter;
                return String.Format("{0}www.{1}{2}", m, domainName, DetailLink);
            }
            else
            {
                return this[StoreConstants.CanonicalUrl];
            }
        }

        public List<Setting> StoreSettings { get; set; }
 
        public String this[String key]
        {
            get
            {
                var item = StoreSettings.FirstOrDefault(r => r.SettingKey.Equals(key, StringComparison.InvariantCultureIgnoreCase));
                if (item != null)
                {
                    return item.SettingValue;
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
