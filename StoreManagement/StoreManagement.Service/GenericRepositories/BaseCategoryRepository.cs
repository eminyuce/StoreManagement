using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Service.GenericRepositories
{
    public class BaseCategoryRepository
    {
        public static List<T> GetBaseCategoriesSearchList<T>(IBaseRepository<T, int> repository, int storeId, string search, String type) where T : BaseCategory
        {
            var cats = repository.FindBy(r => r.StoreId == storeId &&
                                  r.CategoryType.Equals(type, StringComparison.InvariantCultureIgnoreCase));

            if (!String.IsNullOrEmpty(search.ToStr()))
            {
                cats = cats.Where(r => r.Name.ToLower().Contains(search.ToLower().Trim()));
            }

            return cats.OrderBy(r => r.Ordering).ThenByDescending(r => r.Id).ToList();
        }
    }
}
