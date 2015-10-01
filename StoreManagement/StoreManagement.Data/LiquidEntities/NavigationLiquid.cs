using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;

namespace StoreManagement.Data.LiquidEntities
{
    public class NavigationLiquid : BaseDrop
    {
        public Navigation Navigation { get; set; }


        public NavigationLiquid()
        {
            
        }
        
        public NavigationLiquid(Navigation nav)
        {
            this.Navigation = nav;

        }




        public string Link
        {
            get
            {
                return LinkHelper.GetNavigationLink(this.Navigation);
            }
        }
    }
}
