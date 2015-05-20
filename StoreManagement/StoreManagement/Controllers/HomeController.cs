using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using Ninject;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Controllers
{
    public class HomeController : BaseController
    {
        private INavigationRepository _navigationRepository;

        [Inject]
        public ICategoryRepository CategoryRepository { get; set; }
         

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
            var shp =new StoreHomePage();
            shp.Store = store;
            shp.Categories = CategoryRepository.GetCategoriesByStoreId(store.Id);
            return View(shp);
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
