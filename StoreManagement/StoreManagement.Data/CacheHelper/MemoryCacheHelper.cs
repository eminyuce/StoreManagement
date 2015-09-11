using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.CacheHelper
{
    public class MemoryCacheHelper
    {
        public static CacheItemPolicy CacheAbsoluteExpirationPolicy(int minutes)
        {
            return new CacheItemPolicy() { AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddMinutes(minutes)) };
        }

        public static void ClearCache(String cacheKeyPrefix)
        {
            var cacheEnumerator = (IDictionaryEnumerator)((IEnumerable)MemoryCache.Default).GetEnumerator();
            while (cacheEnumerator.MoveNext())
            {
                if (cacheEnumerator.Key.ToString().StartsWith(cacheKeyPrefix, StringComparison.InvariantCultureIgnoreCase))
                {
                    MemoryCache.Default.Remove(cacheEnumerator.Key.ToString());
                }
            }
        }
    }
}
