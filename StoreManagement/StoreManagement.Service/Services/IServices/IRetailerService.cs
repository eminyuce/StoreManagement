using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using StoreManagement.Data.ActionResults;

namespace StoreManagement.Service.Services.IServices
{
    public interface IRetailerService : IBaseService
    {
        SitemapResult RetailersSitemapResult(Controller sitemapsController);
    }
}
