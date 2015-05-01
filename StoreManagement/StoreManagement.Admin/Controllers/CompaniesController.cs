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

namespace StoreManagement.Admin.Controllers
{
    public class CompaniesController : BaseController
    {
        //
        // GET: /Companies/
        private ICompanyRepository companyRepository;
        public CompaniesController(IStoreContext dbContext, ICompanyRepository companyRepository) : base(dbContext)
        {
            this.companyRepository = companyRepository;
        }

        public ActionResult Index(int pageIndex=1, int pageSize=20)
        {
            List<Company> countries = companyRepository.Paginate(pageIndex, pageSize);
            ViewBag.PageIndex = pageIndex;
            return View(countries);
        }
       
	}
}