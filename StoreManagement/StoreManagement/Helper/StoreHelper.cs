using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using StoreManagement.Data;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Helper
{
    public class StoreHelper
    {
        public Store GetStoreByDomain(IStoreService storeService, HttpRequestBase request)
        {
            String siteStatus = ProjectAppSettings.GetWebConfigString("SiteStatus", "dev");

            if (siteStatus.IndexOf("live", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
        
                String domainName = "FUELTECHNOLOGYAGE.COM";
                domainName = request.Url.Scheme + Uri.SchemeDelimiter + request.Url.Host +
                             (request.Url.IsDefaultPort ? "" : ":" + request.Url.Port);
                domainName = GeneralHelper.GetDomainPart(domainName);
                return storeService.GetStore(domainName);
            }
            else
            {
               return storeService.GetStoreByDomain("login.seatechnologyjobs.com");
            }
           
        }
    }
}