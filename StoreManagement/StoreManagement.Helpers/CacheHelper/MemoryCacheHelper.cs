using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Helpers.CacheHelper
{
    public class MemoryCacheHelper
    {
        public static CacheItemPolicy CacheAbsoluteExpirationPolicy(int minutes)
        {
            return new CacheItemPolicy() { AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddMinutes(minutes)) };
        }
    }
}
