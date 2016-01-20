using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Data.RequestModel
{
    public abstract class ViewModel : BaseDrop
    {
        public Store SStore { get; set; }

        public StoreLiquid Store
        {
            get
            {
                return new StoreLiquid(SStore);
            }
        }

        public List<Setting> SSettings { get; set; }

        public List<SettingLiquid> Settings
        {
            get { return SSettings.Select(r => new SettingLiquid(r)).ToList(); }
        }

        public List<Navigation> SNavigations { get; set; }
        public List<NavigationLiquid> Navigations
        {
            get { return SNavigations.Select(r => new NavigationLiquid(r)).ToList(); }
        }

    }
}
