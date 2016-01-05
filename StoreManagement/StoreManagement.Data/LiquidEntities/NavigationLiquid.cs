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

        public int Id
        {
            get { return Navigation.Id; }
        }
        public String Name
        {
            get { return Navigation.Name; }
        }

        public DateTime CreatedDate
        {
            get { return Navigation.CreatedDate.Value; }
        }
        public DateTime UpdatedDate
        {
            get { return Navigation.UpdatedDate.Value; }
        }
        public bool State
        {
            get { return Navigation.State; }
        }



    }
}
