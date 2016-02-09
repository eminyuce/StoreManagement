using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using StoreManagement.Data.ActionResults;

namespace StoreManagement.Service.Services.IServices
{
    public interface ISiteMapService : IBaseService
    {
        SitemapResult GetNavigationSiteMap(Controller sitemapsController);
        SitemapResult GetProductsSiteMap(Controller sitemapsController);
        SitemapResult GetProductCategoriesSiteMap(Controller sitemapsController);
        SitemapResult GetBrandsSiteMap(Controller sitemapsController);
        SitemapResult RetailersSitemapResult(Controller sitemapsController);
    }
}
