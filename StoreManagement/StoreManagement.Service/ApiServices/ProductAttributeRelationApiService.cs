using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.ApiServices
{
    public class ProductAttributeRelationApiService : BaseApiService, IProductAttributeRelationService
    {
        public ProductAttributeRelationApiService(string webServiceAddress) : base(webServiceAddress)
        {
        }

        protected override string ApiControllerName
        {
            get { return "ProductAttributeRelations"; }
        }

        protected override void SetCache()
        {
            HttpRequestHelper.CacheMinute = CacheMinute;
            HttpRequestHelper.IsCacheEnable = IsCacheEnable;
        }
    }
}
