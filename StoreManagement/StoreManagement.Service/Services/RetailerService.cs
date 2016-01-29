using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using NLog;
using StoreManagement.Data;
using StoreManagement.Data.ActionResults;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.SEO;
using StoreManagement.Service.Services.IServices;

namespace StoreManagement.Service.Services
{
    public class RetailerService : BaseService, IRetailerService
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly TypedObjectCache<List<SitemapItem>> RetailerSitemapItemCache = new TypedObjectCache<List<SitemapItem>>("RetailerSitemapItemCache");

        public SitemapResult RetailersSitemapResult(Controller sitemapsController)
        {
            var sitemapItems = new List<SitemapItem>();

            String key = String.Format("RetailersSiteMap-{0}", StoreId);

            RetailerSitemapItemCache.TryGet(key, out sitemapItems);

            if (sitemapItems == null)
            {
                sitemapItems = new List<SitemapItem>();
                var retailers = RetailerRepository.GetRetailers(MyStore.Id, null, true);
                foreach (var retailer in retailers)
                {
                    var retailerDetailLink = LinkHelper.GetRetailerIdRouteValue(retailer);
                    var siteMap = new SitemapItem(sitemapsController.Url.AbsoluteAction("detail", "retailers", new { id = retailerDetailLink }),
                                         changeFrequency: SitemapChangeFrequency.Monthly, priority: 1.0);
                    sitemapItems.Add(siteMap);

                }
                RetailerSitemapItemCache.Set(key, sitemapItems, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.CacheLongSeconds));
            }
            return new SitemapResult(sitemapItems);
        }
    }
}
