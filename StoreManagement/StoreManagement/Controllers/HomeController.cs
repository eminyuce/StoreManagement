using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Controllers
{
    public class HomeController : BaseController
    {
        private INavigationRepository _navigationRepository;
        public HomeController(IStoreContext dbContext, 
            ISettingRepository settingRepository,
            IStoreRepository storeRepository,
            INavigationRepository navigationRepository)
            : base(dbContext, settingRepository, storeRepository)
        {
            _navigationRepository = navigationRepository;
        }

        public ActionResult Index()
        {
            ViewBag.Store = store;
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult RecentUpdates()
        {
           return View();
        }
        public ActionResult MainMenu()
        {
            var mainMenu = _navigationRepository.GetStoreNavigation(store.Id);
            return View(mainMenu);
        }
        public ActionResult Footer()
        {
            return View();
        }
    }
}
