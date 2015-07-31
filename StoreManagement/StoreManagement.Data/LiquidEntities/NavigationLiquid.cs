using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;

namespace StoreManagement.Data.LiquidEntities
{
    public class NavigationLiquid
    {
        public Navigation Navigation;
        public PageDesign PageDesign;

        public NavigationLiquid()
        {
            
        }
        
        public NavigationLiquid(Navigation nav, PageDesign pageDesign)
        {
            this.Navigation = nav;
            this.PageDesign = pageDesign;
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
