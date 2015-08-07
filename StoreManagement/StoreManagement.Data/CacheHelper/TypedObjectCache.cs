using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Caching;

namespace StoreManagement.Data.CacheHelper
{

    public class TypedObjectCache<T> : MemoryCache where T : class
    {
        private bool _isCacheEnable = true;
        public bool IsCacheEnable
        {
            get { return _isCacheEnable; }
            set { _isCacheEnable = value; }
        }
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
            if (IsCacheEnable)
            {
                policy = policy ?? defaultCacheItemPolicy;
                if (true /* Ektron.Com.Helpers.Constants.IsCachingEnabled */ )
                {
                    var isCount = 0;
                    if (cacheItem is IList)
                    {
                        isCount = (cacheItem as IList).Count;
                    }
                    if (isCount != 0)  // No need to cache it
                    {

                        base.Set(cacheKey, cacheItem, policy);
                    }
                }
            }
        }

        public void Set(string cacheKey, Func<T> getData, CacheItemPolicy policy = null)
        {
            if (IsCacheEnable)
            {
                this.Set(cacheKey, getData(), policy);
            }
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
            if (IsCacheEnable)
            {
                returnItem = (T) this[cacheKey];
                return returnItem != null;
            }
            else
            {
                returnItem = null;
                return false;
            }
        }
        public void ClearCache(String cacheKeyPrefix)
        {
            var cacheEnumerator = (IDictionaryEnumerator)((IEnumerable)Default).GetEnumerator();
            while (cacheEnumerator.MoveNext())
            {
                if (cacheEnumerator.Key.ToString().StartsWith(cacheKeyPrefix, StringComparison.InvariantCultureIgnoreCase))
                {
                    Default.Remove(cacheEnumerator.Key.ToString());
                }
            }
        }
    }
}
