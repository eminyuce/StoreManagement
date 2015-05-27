using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.HelpersModel;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Services
{
    public class CompanyService : BaseService, ICompanyService
    {
        public CompanyService(string webServiceAddress) : base(webServiceAddress)
        {

        }

        public CompanySearchResult GetCompanySearchResult(string search, List<Filter> filters, int take, int skip)
        {
            throw new NotImplementedException();
        }
    }
}
