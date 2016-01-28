using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.ApiServices
{
    public class EmailListApiService : BaseApiService, IEmailListService
    {

        protected override string ApiControllerName { get { return "EmailLists"; } }


        public EmailListApiService(string webServiceAddress) : base(webServiceAddress)
        {

        }

        public List<EmailList> GetStoreEmailList(int storeId)
        {
            throw new NotImplementedException();
        }



        protected override void SetCache()
        {
            HttpRequestHelper.CacheMinute = CacheMinute;
            HttpRequestHelper.IsCacheEnable = IsCacheEnable;
        }
    }
}
