using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using StoreManagement.Service.Services.IServices;

namespace StoreManagement.Service.Services
{
    public class LocationService : BaseService, ILocationService
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    }
}
