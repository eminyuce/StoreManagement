using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Service.IGeneralRepositories;

namespace StoreManagement.Service.ApiRepositories
{

    public class ItemFileApiRepository : BaseApiRepository, IItemFileGeneralRepository
    {

        protected override string ApiControllerName { get { return "ItemFiles"; } }


        public ItemFileApiRepository(string webServiceAddress) : base(webServiceAddress)
        {

        }

 

        protected override void SetCache()
        {
            HttpRequestHelper.CacheMinute = CacheMinute;
            HttpRequestHelper.IsCacheEnable = IsCacheEnable;
        }
    }
}
