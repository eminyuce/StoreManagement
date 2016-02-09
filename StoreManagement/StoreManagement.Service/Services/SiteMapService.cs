using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using StoreManagement.Data;
using StoreManagement.Data.ActionResults;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.Constants;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.SEO;
using StoreManagement.Service.Services.IServices;

namespace StoreManagement.Service.Services
{
    public class SiteMapService : BaseService, ISiteMapService
    {

        private static readonly TypedObjectCache<List<SitemapItem>> RetailerSitemapItemCache = new TypedObjectCache<List<SitemapItem>>("RetailerSitemapItemCache");


        public SitemapResult GetNavigationSiteMap(Controller sitemapsController)
        {
            var sitemapItems = new List<SitemapItem>();
            var navigations = NavigationRepository.GetStoreActiveNavigations(this.MyStore.Id);
            foreach (var navigation in navigations)
            {
                var siteMap = new SitemapItem(sitemapsController.Url.QualifiedAction(navigation.ActionName, navigation.ControllerName),
                                     changeFrequency: SitemapChangeFrequency.Monthly, priority: 1.0);
                sitemapItems.Add(siteMap);
            }

            return new SitemapResult(sitemapItems);
        }

        public SitemapResult GetProductsSiteMap(Controller sitemapsController)
        {
            var sitemapItems = new List<SitemapItem>();

            sitemapItems = new List<SitemapItem>();
            var products = ProductRepository.GetProductByTypeAndCategoryIdFromCache(StoreId, StoreConstants.ProductType, -1);
            var categories = ProductCategoryRepository.GetProductCategoriesByStoreIdFromCache(StoreId,
                                                                                           StoreConstants.ProductType);
            foreach (var product in products)
            {
                var cat = categories.FirstOrDefault(r => r.Id == product.ProductCategoryId);
                if (cat != null)
                {
                    var productDetailLink = LinkHelper.GetProductIdRouteValue(product, cat.Name);
                    var siteMap = new SitemapItem(sitemapsController.Url.AbsoluteAction("Product", "Products", new { id = productDetailLink }),
                                         changeFrequency: SitemapChangeFrequency.Monthly, priority: 1.0);
                    sitemapItems.Add(siteMap);
                }
            }
            return new SitemapResult(sitemapItems);
        }

        public SitemapResult GetProductCategoriesSiteMap(Controller sitemapsController)
        {
            var sitemapItems = new List<SitemapItem>();
            sitemapItems = new List<SitemapItem>();
            var categories = ProductCategoryRepository.GetProductCategoriesByStoreIdFromCache(StoreId,
                                                                                               StoreConstants.ProductType);
            foreach (var category in categories)
            {
                var productDetailLink = LinkHelper.GetProductCategoryIdRouteValue(category);
                var siteMap = new SitemapItem(sitemapsController.Url.AbsoluteAction("category", "productcategories", new { id = productDetailLink }),
                                     changeFrequency: SitemapChangeFrequency.Monthly, priority: 1.0);
                sitemapItems.Add(siteMap);

            }
            return new SitemapResult(sitemapItems);

        }

        public SitemapResult GetBrandsSiteMap(Controller sitemapsController)
        {
            var sitemapItems = new List<SitemapItem>();
            sitemapItems = new List<SitemapItem>();


            var brands = BrandRepository.GetBrands(StoreId, null, true);
            foreach (var brand in brands)
            {
                var brandDetailLink = LinkHelper.GetBrandIdRouteValue(brand);
                var siteMap = new SitemapItem(sitemapsController.Url.AbsoluteAction("detail", "brands", new { id = brandDetailLink }),
                                     changeFrequency: SitemapChangeFrequency.Monthly, priority: 1.0);
                sitemapItems.Add(siteMap);
            }

            return new SitemapResult(sitemapItems);
        }

      
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
