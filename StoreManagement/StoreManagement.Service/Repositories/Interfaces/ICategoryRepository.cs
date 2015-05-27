using System;
using System.Collections.Generic;
using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface ICategoryRepository : IEntityRepository<Category, int>, ICategoryService
    {
       

    }

}
