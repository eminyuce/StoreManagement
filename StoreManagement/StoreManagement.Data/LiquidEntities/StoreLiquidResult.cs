using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Constants;

namespace StoreManagement.Data.LiquidEntities
{
    public class StoreLiquidResult
    {
        public int StoreId { get; set; }
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
                return String.Format("<link rel='canonical' href='{0}{1}{2}' >", m, domainName, DetailLink);
            }
            else
            {
                return "";
            }
        }
    }
}
