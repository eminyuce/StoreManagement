using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.ApiRepositories
{
    public class StoreLanguageApiRepository : BaseApiRepository, IStoreLanguageService
    {
        public StoreLanguageApiRepository(string webServiceAddress) : base(webServiceAddress)
        {

        }

        protected override string ApiControllerName { get { return "StoreLanguages"; } }

        protected override void SetCache()
        {
            HttpRequestHelper.CacheMinute = CacheMinute;
            HttpRequestHelper.IsCacheEnable = IsCacheEnable;
        }
    }
}
