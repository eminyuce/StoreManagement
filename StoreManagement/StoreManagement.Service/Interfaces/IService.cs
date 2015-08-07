using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data;

namespace StoreManagement.Service.Interfaces
{
    public interface IService
    {
        bool IsCacheEnable { get; set; }
        int CacheMinute { get; set; }
       
    }
}
