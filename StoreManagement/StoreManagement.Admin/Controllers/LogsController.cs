using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using StoreManagement.Data;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.LogEntities;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class LogsController : BaseController
    {

        [Inject]
        public ILogRepository LogRepository { set; get; }

        public ActionResult Index()
        {
            var item = LogRepository.GetApplicationNames();
            return View(item);
        }
        [HttpGet]
        public ActionResult AppLogDetail(String appName = "", String logLevel = "", String page = "", String search = "")
        {

            int iPage = page.ToInt(); if (iPage == 0) iPage = 1;

            int top = 200;
            int skip = (iPage - 1) * top;

            var item = LogRepository.GetApplicationLogs(appName, logLevel, top, skip, search);
            if (!String.IsNullOrEmpty(appName))
            {
                item.ApplicationName = appName;
            }
            ViewBag.CurrentLogLevelText = logLevel;
            ViewBag.CurrentLogLevel = (int)(LogLevels)Enum.Parse(typeof(LogLevels), logLevel);

            return View(item);
        }

        public ActionResult DeleteLogs(string id = "", String logLevel = "Trace")
        {
            var application = id;
            ViewBag.ApplicationName = id;
            int iPage = 1;
            int top = ProjectAppSettings.RecordPerPage * 15;
            int skip = (iPage - 1) * top;
            var item = LogRepository.GetApplicationLogs(application, logLevel, top, skip, "");
            return View(item);
        }
        public ActionResult RemoveLogs(string id = "", String logLevel = "")
        {
            var application = id;
            String logLevel2 = logLevel.ToInt() > 0 ? ((LogLevels)logLevel.ToInt()).ToString() : logLevel;
            LogRepository.DeleteLogs(application, logLevel2);
            Logger.Trace(String.Format("ApplicationName {0} Log Level {1}", id, logLevel));
            return RedirectToAction("DeleteLogs", new { id = id });
        }

        public ActionResult TotalSpace(string id = "")
        {
            var list = LogRepository.GetTotalTablesSpace();
            return View(list);
        }
    }
}