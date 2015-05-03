using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Data.RequestModel;
using System.Collections.Generic;
using Filter = StoreManagement.Data.HelpersModel.Filter;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface ICompanyRepository : IEntityRepository<Company, int>
    {
        CompanySearchResult GetCompanySearchResult(string search, List<Filter> filters, int take, int skip);
    }
}
