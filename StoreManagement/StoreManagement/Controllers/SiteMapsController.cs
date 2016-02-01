using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NLog;
using StoreManagement.Data;
using StoreManagement.Data.ActionResults;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.Constants;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.SEO;

namespace StoreManagement.Controllers
{
    [OutputCache(CacheProfile = "Cache10Days")]
    public class SitemapsController : BaseController
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly TypedObjectCache<List<SitemapItem>> ProductSitemapItemCache = new TypedObjectCache<List<SitemapItem>>("ProductSitemapItemCache");
        //
        // GET: /Sitemap/
        public ActionResult Index()
        {
            var sitemapItems = new List<SitemapItem>();
            var navigations = NavigationService.GetStoreActiveNavigations(this.MyStore.Id);
            foreach (var navigation in navigations)
            {
                var siteMap = new SitemapItem(Url.QualifiedAction(navigation.ActionName, navigation.ControllerName),
                                     changeFrequency: SitemapChangeFrequency.Monthly, priority: 1.0);
                sitemapItems.Add(siteMap);
            }

            return new SitemapResult(sitemapItems);
        }
        public ActionResult Products()
        {
            var sitemapItems = new List<SitemapItem>();

            String key = String.Format("ProductSitemapItemCache-{0}", StoreId);

            ProductSitemapItemCache.TryGet(key, out sitemapItems);

            if (sitemapItems == null)
            {
                sitemapItems = new List<SitemapItem>();
                var products = ProductRepository.GetProductByTypeAndCategoryIdFromCache(StoreId, StoreConstants.ProductType, -1);
                var categories = ProductCategoryService.GetProductCategoriesByStoreIdFromCache(StoreId,
                                                                                               StoreConstants.ProductType);
                foreach (var product in products)
                {
                    var cat = categories.FirstOrDefault(r => r.Id == product.ProductCategoryId);
                    if (cat != null)
                    {
                        var productDetailLink = LinkHelper.GetProductIdRouteValue(product, cat.Name);
                        var siteMap = new SitemapItem(Url.AbsoluteAction("Product", "Products", new { id = productDetailLink }),
                                             changeFrequency: SitemapChangeFrequency.Monthly, priority: 1.0);
                        sitemapItems.Add(siteMap);
                    }

                }
                ProductSitemapItemCache.Set(key, sitemapItems, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.CacheLongSeconds));
            }
            return new SitemapResult(sitemapItems);
        }
        public ActionResult ProductCategories()
        {
            var sitemapItems = new List<SitemapItem>();

            String key = String.Format("ProductCategoriesSiteMap-{0}", StoreId);

            ProductSitemapItemCache.TryGet(key, out sitemapItems);

            if (sitemapItems == null)
            {
                sitemapItems = new List<SitemapItem>();
                var categories = ProductCategoryService.GetProductCategoriesByStoreIdFromCache(StoreId,
                                                                                               StoreConstants.ProductType);
                foreach (var category in categories)
                {
                    var productDetailLink = LinkHelper.GetProductCategoryIdRouteValue(category);
                    var siteMap = new SitemapItem(Url.AbsoluteAction("category", "productcategories", new { id = productDetailLink }),
                                         changeFrequency: SitemapChangeFrequency.Monthly, priority: 1.0);
                    sitemapItems.Add(siteMap);

                }
                ProductSitemapItemCache.Set(key, sitemapItems, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.CacheLongSeconds));
            }
            return new SitemapResult(sitemapItems);
        }
        public async Task<ActionResult> Brands()
        {
            var sitemapItems = new List<SitemapItem>();

            String key = String.Format("BrandsSiteMap-{0}", StoreId);

            ProductSitemapItemCache.TryGet(key, out sitemapItems);

            if (sitemapItems == null)
            {
                sitemapItems = new List<SitemapItem>();
                var brandsTask = BrandService.GetBrandsAsync(StoreId, null, true);
                await Task.WhenAll(brandsTask);
                var brands = brandsTask.Result;
                foreach (var brand in brands)
                {
                    var brandDetailLink = LinkHelper.GetBrandIdRouteValue(brand);
                    var siteMap = new SitemapItem(Url.AbsoluteAction("detail", "brands", new { id = brandDetailLink }),
                                         changeFrequency: SitemapChangeFrequency.Monthly, priority: 1.0);
                    sitemapItems.Add(siteMap);

                }
                ProductSitemapItemCache.Set(key, sitemapItems, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.CacheLongSeconds));
            }
            return new SitemapResult(sitemapItems);
        }
        public ActionResult Retailers()
        {
            return RetailerService2.RetailersSitemapResult(this);
        }
    }
}