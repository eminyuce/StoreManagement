using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework.Enums;
using StoreManagement.Data.Entities;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Service.Repositories
{
    public class CommentRepository : BaseRepository<Comment, int>, ICommentRepository
    {
        public CommentRepository(IStoreContext dbContext)
            : base(dbContext)
        {

        }

        public Task<List<Comment>> GetCommentsByItemIdAsync(int storeId, int itemId, string itemType, int page, int pageSize)
        {
            try
            {
                Expression<Func<Comment, bool>> match = r2 => r2.StoreId == storeId && r2.State && r2.ItemId == itemId && r2.ItemType.Equals(itemType, StringComparison.InvariantCultureIgnoreCase);
                Expression<Func<Comment, int>> keySelector = t => t.Id;
                var items = this.FindAllAsync(match, keySelector, OrderByType.Descending, page, pageSize);
                return items;
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }
        }
    }
}
