using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework.Enums;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Service.GenericRepositories
{
    public class BaseContentRepository : GenericBaseRepository
    {
        public static Task<List<T>> GetActiveBaseContentsAsync<T>(IBaseRepository<T, int> repository, int storeId, int? take, bool? isActive) where T : BaseContent
        {
            try
            {
                Expression<Func<T, bool>> match = r2 => r2.StoreId == storeId && r2.State == (isActive.HasValue ? isActive.Value : r2.State);
                var items = repository.FindAllAsync(match, t => t.Ordering, OrderByType.Descending, take);
                var itemsResult = items;
                return itemsResult;
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }
        }
    }
}
