using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.HelpersModel;
using StoreManagement.Data.RequestModel;

namespace StoreManagement.Service.Interfaces
{
    public interface ICompanyService
    {
        CompanySearchResult GetCompanySearchResult(string search, List<Filter> filters, int take, int skip);
    }
}
