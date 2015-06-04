using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using StoreManagement.Data.GeneralHelper;

namespace StoreManagement.Service.Services
{
    public abstract class BaseService
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected string WebServiceAddress { get; set; }


        protected RequestHelper RequestHelper;
        public BaseService()
        {
            RequestHelper = new RequestHelper();
        }

        protected BaseService(String webServiceAddress):this()
        {
            WebServiceAddress = webServiceAddress;
        }

       
    }
}
