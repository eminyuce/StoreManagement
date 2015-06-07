using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;
using GenericRepository.EntityFramework;
using StoreManagement.Data;
using StoreManagement.Service.DbContext;

namespace StoreManagement.Service.Repositories
{
    public abstract class BaseRepository<T, TId> : EntityRepository<T, TId> where T : class, IEntity<TId> where TId : IComparable
    {
        protected bool IsCacheActive
        {
            get
            {
                return ProjectAppSettings.GetWebConfigBool("IsCacheActive", true);
            }
        }
        protected StoreContext StoreDbContext;
        protected BaseRepository(IStoreContext dbContext) : base(dbContext)
        {
            StoreDbContext =   (StoreContext) dbContext;
            StoreDbContext.Configuration.LazyLoadingEnabled = false; 
        }
         
    }
}
