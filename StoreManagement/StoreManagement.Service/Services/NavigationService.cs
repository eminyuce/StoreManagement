using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using StoreManagement.Service.Services.IServices;

namespace StoreManagement.Service.Services
{
    public class NavigationService : BaseService, INavigationService
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public bool IsModulActive(string controllerName)
        {
            var navigations = NavigationRepository.GetStoreActiveNavigations(MyStore.Id);
           return navigations.Any(r => r.ControllerName.ToLower().StartsWith(controllerName.ToLower()));
        }
    }
}
