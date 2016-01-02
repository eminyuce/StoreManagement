using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Data.LiquidEntities
{
    public class StoreLiquid
    {
        private Store Store { get; set; }
        public StoreLiquid(Store store)
        {
            this.Store = store;
        }
    }
}
