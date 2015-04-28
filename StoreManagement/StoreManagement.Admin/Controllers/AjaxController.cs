using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Admin.Controllers
{
    public class AjaxController : BaseController
    {
        //
        // GET: /Ajax/

        private IStoreContext dbContext;
        private IStoreRepository storeRepository;
        private ISettingRepository settingRepository;
        public AjaxController(IStoreContext dbContext, IStoreRepository storeRepository, ISettingRepository settingRepository)
            : base(dbContext)
        {
            this.dbContext = dbContext;
            this.storeRepository = storeRepository;
            this.settingRepository = settingRepository;
        }


        public ActionResult SaveSettingValue(int id = 0, string value = "")
        {
            var s = settingRepository.GetSingle(id);
            s.SettingValue = value;
            settingRepository.Edit(s);
            settingRepository.Save();
            return Content(value);
        }

    }
}
