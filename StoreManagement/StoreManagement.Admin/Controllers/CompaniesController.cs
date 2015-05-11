using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GenericRepository;
using StoreManagement.Data.Entities;
using StoreManagement.Data.Paging;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data;

namespace StoreManagement.Admin.Controllers
{
    //[Authorize]
    public class CompaniesController : BaseController
    {
        //
        // GET: /Companies/
        private ICompanyRepository companyRepository;

        public CompaniesController(IStoreContext dbContext, 
            ISettingRepository settingRepository,
            ICompanyRepository companyRepository)
            : base(dbContext, settingRepository)
        {
            this.companyRepository = companyRepository;
        }

        public ActionResult Company(String id = "1")
        {
            int companyid = id.Split("-".ToCharArray()).Last().ToInt();
            var c = this.companyRepository.GetSingle(companyid);
            return View(c);
        }
        public ActionResult Index(int pageIndex=1, int pageSize=20)
        {
            List<Company> countries = companyRepository.Paginate(pageIndex, pageSize);
            ViewBag.PageIndex = pageIndex;
            return View(countries);
        }
        public ActionResult CompaniesSearch(String search="", String page="1", String filters="")
        {
            int iPage = page.ToInt(); if (iPage == 0) iPage = 1;
            int top = ProjectAppSettings.RecordPerPage;
            int skip = (iPage - 1) * top;
            
            var fltrs = FilterHelper.ParseFiltersFromString(filters);
            var searchResult = companyRepository.GetCompanySearchResult(search, fltrs, top, skip);

            return View(searchResult);
        }
        public ActionResult SaveOrUpdate(int id = 1)
        {
            int companyid = id;
            var c = this.companyRepository.GetSingle(companyid);
            return View(c);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveOrUpdate(Company company)
        {
            if (ModelState.IsValid)
            {
                companyRepository.Edit(company);
                companyRepository.Save();
                return RedirectToAction("CompaniesSearch");
            }
            return View(company);
        }

        public ActionResult GetCompanies()
        {
            return View(companyRepository.GetAll().ToList());
        }
	}
}