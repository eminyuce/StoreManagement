using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.ApiServices
{
    public class ProductAttributeApiService: BaseApiService, IProductAttributeService
    {
        protected override string ApiControllerName { get { return "ProductAttributeServices"; } }
        public ProductAttributeApiService(string webServiceAddress)
            : base(webServiceAddress)
        {

        }


        protected override void SetCache()
        {
            HttpRequestHelper.CacheMinute = CacheMinute;
            HttpRequestHelper.IsCacheEnable = IsCacheEnable;
        }
    }
}
