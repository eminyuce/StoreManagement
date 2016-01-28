using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.ApiServices
{
    public class MessageApiService : BaseApiService, IMessageService
    {
        public MessageApiService(string webServiceAddress) : base(webServiceAddress)
        {
        }

        protected override string ApiControllerName
        {
            get { return "Messages"; }
        }



        protected override void SetCache()
        {
            HttpRequestHelper.CacheMinute = CacheMinute;
            HttpRequestHelper.IsCacheEnable = IsCacheEnable;
        }

        public void SaveContactFormMessage(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
