using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Service.Services
{
    public abstract class BaseService
    {
        protected string WebServiceAddress { get; set; }
        protected BaseService(String webServiceAddress)
        {
            WebServiceAddress = webServiceAddress;
        }
    }
}
