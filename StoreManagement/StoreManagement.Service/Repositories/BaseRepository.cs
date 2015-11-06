using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;
using GenericRepository.EntityFramework;
using NLog;
using StoreManagement.Data;
using StoreManagement.Data.Entities;
using StoreManagement.Service.DbContext;

namespace StoreManagement.Service.Repositories
{
    public abstract class BaseRepository<T, TId> : EntityRepository<T, TId>
        where T : class, IEntity<TId>
        where TId : IComparable
    {
        
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private bool _isCacheEnable = true;
        public bool IsCacheEnable
        {
            get { return _isCacheEnable; }
            set { _isCacheEnable = value; }
        }
        private int _cacheMinute = 30;
        public int CacheMinute
        {
            get { return _cacheMinute; }
            set { _cacheMinute = value; }
        }
        protected IStoreContext DbContext;
        protected StoreContext StoreDbContext
        {
            get
            {
                return (StoreContext)DbContext;
            }
        }
        
        protected BaseRepository(IStoreContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
            StoreDbContext.Configuration.LazyLoadingEnabled = false;
        }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.DbContext.Dispose();
                }
            }
            this.disposed = true;
        }

    }
}
