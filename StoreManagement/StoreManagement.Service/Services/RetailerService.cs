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
     
    }
}
