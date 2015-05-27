using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Data.RequestModel;
using System.Collections.Generic;
using StoreManagement.Service.Interfaces;
using Filter = StoreManagement.Data.HelpersModel.Filter;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface ICompanyRepository : IEntityRepository<Company, int>, ICompanyService
    {

    }
}
