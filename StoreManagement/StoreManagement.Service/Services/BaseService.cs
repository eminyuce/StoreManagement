using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace StoreManagement.Service.Services
{
    public abstract class BaseService
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        protected string WebServiceAddress { get; set; }
        protected BaseService(String webServiceAddress)
        {
            WebServiceAddress = webServiceAddress;
        }

    }
}
