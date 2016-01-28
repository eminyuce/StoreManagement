using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Data.Paging;
using StoreManagement.Service.IGeneralRepositories;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category, int>, ICategoryGeneralRepository, IDisposable 
    {
      
    }

}
