using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface ICategoryRepository : IEntityRepository<Category, int>
    {
        List<Category> GetCategoriesByStoreId(int storeId, String type);
        List<Category> GetCategoriesByStoreIdFromCache(int storeId, String type);
        List<Category> CreateCategoriesTree(int storeId, String type);


    }

}
