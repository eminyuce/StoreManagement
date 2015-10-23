using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;

namespace StoreManagement.Data.LiquidEntities
{
    public class RetailerLiquid : BaseDrop
    {
        public Retailer Retailer { get; set; } 
        public RetailerLiquid(Retailer retailer)
        {
            Retailer = retailer;
        }


        public string Link
        {
            get { return LinkHelper.GetRetailerLink(this.Retailer); }
        }
    }
}
