using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;
using GenericRepository.EntityFramework;
using NLog;
using StoreManagement.Data;
using StoreManagement.Service.DbContext;

namespace StoreManagement.Service.Repositories
{
    public abstract class BaseRepository<T, TId> : EntityRepository<T, TId> where T : class, IEntity<TId> where TId : IComparable
    {

        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        //protected bool IsCacheActive
        //{
        //    get
        //    {
        //        return ProjectAppSettings.GetWebConfigBool("IsCacheActive", true);
        //    }
        //}
        protected StoreContext StoreDbContext;
        protected BaseRepository(IStoreContext dbContext) : base(dbContext)
        {
            StoreDbContext =   (StoreContext) dbContext;
            StoreDbContext.Configuration.LazyLoadingEnabled = false; 
        }
         
    }
}
