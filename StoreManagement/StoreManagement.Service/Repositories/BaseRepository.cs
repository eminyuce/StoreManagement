using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;
using GenericRepository.EntityFramework;
using StoreManagement.Service.DbContext;

namespace StoreManagement.Service.Repositories
{
    public abstract class BaseRepository<T, TId> : EntityRepository<T, TId> where T : class, IEntity<TId> where TId : IComparable
    {
        protected IStoreContext StoreDbContext;
        protected BaseRepository(IStoreContext dbContext) : base(dbContext)
        {
            StoreDbContext = dbContext;
        }
         
    }
}
