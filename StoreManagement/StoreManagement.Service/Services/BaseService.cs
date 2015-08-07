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

        protected RequestHelper HttpRequestHelper;

        private bool _isCacheEnable = true;
        public bool IsCacheEnable
        {
            get { return _isCacheEnable; }
            set { _isCacheEnable = value; }
        }
        private int _cacheMinute = 30;
        public int CacheMinute
        {
            get { return _cacheMinute; }
            set { _cacheMinute = value; }
        }

        protected BaseService(String webServiceAddress)
        {
            WebServiceAddress = webServiceAddress;
            HttpRequestHelper = new RequestHelper();
            HttpRequestHelper.CacheMinute = this.CacheMinute;
            HttpRequestHelper.IsCacheEnable = IsCacheEnable;
        }

       
    }
}
