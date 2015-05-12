using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using NLog;
using Ninject;
using StoreManagement.Data;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Models
{
    public class StoreHelper
    {
        private IStoreRepository _storeRepository { set; get; }
        public StoreHelper(IStoreRepository storeRepository)
        {
            _storeRepository=storeRepository;
        }

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static string defaultlayout = "~/Views/Shared/Layouts/{0}.cshtml";
      

        public Store GetStore(HttpRequest Request)
        {
            String domainName = "FUELTECHNOLOGYAGE.COM";
            domainName = Request.Url.Scheme + Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
            domainName = GeneralHelper.GetDomainPart(domainName);
            Logger.Info("Domain name="+domainName);
            Store site = _storeRepository.GetStoreByDomain(domainName);
            string layout = String.Format("~/Views/Shared/Layouts/{0}.cshtml", !String.IsNullOrEmpty((String)site.Layout) ? (String)site.Layout : "_Layout1");
            var isFileExist = File.Exists(System.Web.HttpContext.Current.Server.MapPath(layout));
            defaultlayout = String.Format(defaultlayout, ProjectAppSettings.GetWebConfigString("DefaultLayout", "_Layout1"));
            if (!isFileExist)
            {
                Logger.Info(String.Format("Layout is not found.Default Layout {0} is used.Site Domain is {1} ", defaultlayout, site.Domain));
            }
            String selectedLayout = isFileExist ? layout : defaultlayout;

            site.Layout = selectedLayout;

            Logger.Info("Selected site="+site);


            return site;
        }
    }
}