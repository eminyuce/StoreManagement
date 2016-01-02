using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Data.LiquidEntities
{
    public class StoreLiquid:BaseDrop
    {
        private Store Store { get; set; }
        public StoreLiquid(Store store)
        {
            this.Store = store;
        }

        public String Name
        {
            get { return this.Store.Name; }
        }
    }
}
