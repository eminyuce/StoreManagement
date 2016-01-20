using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Data.LiquidEntities
{
    public class SettingLiquid  : BaseDrop
    {
        public Setting Setting { set; get; }

        public SettingLiquid(Setting setting)
        {
            this.Setting = setting;
        }

        public String Key
        {
            get { return Setting.SettingKey; }
        }
        public String Value
        {
            get { return Setting.SettingKey; }
        }
    }
}
