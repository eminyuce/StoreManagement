using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.ApiServices
{
    public class ContentFileApiService : BaseApiService, IContentFileService
    {
        protected override string ApiControllerName { get { return "ContentFiles"; } }


        public ContentFileApiService(string webServiceAddress) : base(webServiceAddress)
        {

        }

        protected override void SetCache()
        {
            HttpRequestHelper.CacheMinute = CacheMinute;
            HttpRequestHelper.IsCacheEnable = IsCacheEnable;
        }
    }
}
