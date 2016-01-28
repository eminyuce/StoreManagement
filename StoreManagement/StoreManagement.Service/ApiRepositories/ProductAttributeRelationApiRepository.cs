using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.ApiRepositories
{
    public class ProductAttributeRelationApiRepository : BaseApiRepository, IProductAttributeRelationService
    {
        public ProductAttributeRelationApiRepository(string webServiceAddress) : base(webServiceAddress)
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
