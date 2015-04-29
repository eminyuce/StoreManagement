using System;
using System.Collections.Specialized;
using System.Runtime.Caching;

namespace StoreManagement.Helpers.CacheHelper
{

    public class TypedObjectCache<T> : MemoryCache where T : class
    {
        private CacheItemPolicy HardDefaultCacheItemPolicy = new CacheItemPolicy()
        {
            SlidingExpiration = new TimeSpan(0, 15, 0)
        };

        private CacheItemPolicy defaultCacheItemPolicy;

        public TypedObjectCache(string name, NameValueCollection nvc = null, CacheItemPolicy policy = null)
            : base(name, nvc)
        {
            defaultCacheItemPolicy = policy ?? HardDefaultCacheItemPolicy;
        }

        public void Set(string cacheKey, T cacheItem, CacheItemPolicy policy = null)
        {
            policy = policy ?? defaultCacheItemPolicy;
            if (true /* Ektron.Com.Helpers.Constants.IsCachingEnabled */ )
            {
                base.Set(cacheKey, cacheItem, policy);
            }
        }

        public void Set(string cacheKey, Func<T> getData, CacheItemPolicy policy = null)
        {
            this.Set(cacheKey, getData(), policy);
        }

        public bool TryGetAndSet(string cacheKey, Func<T> getData, out T returnData, CacheItemPolicy policy = null)
        {
            if (TryGet(cacheKey, out returnData))
            {
                return true;
            }
            returnData = getData();
            this.Set(cacheKey, returnData, policy);
            return returnData != null;
        }

        public bool TryGet(string cacheKey, out T returnItem)
        {
            returnItem = (T)this[cacheKey];
            return returnItem != null;
        }

    }
}
