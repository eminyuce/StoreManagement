using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;
using Ninject;
using StoreManagement.Data;
using StoreManagement.Data.EmailHelper;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Liquid.Helper
{
    public abstract class BaseLiquidHelper
    {

        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public List<Setting> StoreSettings { get; set; }

        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }

        public int StoreId { get; set; }

        protected bool GetSettingValueBool(String key, bool defaultValue)
        {
            String d = defaultValue ? bool.TrueString : bool.FalseString;
            return GetSettingValue(key, d).ToBool();
        }
        protected int GetSettingValueInt(String key, int defaultValue)
        {
            String d = defaultValue + "";
            return GetSettingValue(key, d).ToInt();
        }
        protected String GetSettingValue(String key, String defaultValue)
        {
            var value = GetSettingValue(key);
            if (String.IsNullOrEmpty(value))
            {
                Logger.Trace("Store Default Setting= " + StoreId + " Key=" + key + " defaultValue=" + defaultValue);
                return ProjectAppSettings.GetWebConfigString(key, defaultValue);
            }
            else
            {
                return value;
            }
        }
        protected String GetSettingValue(String key)
        {
            try
            {
                if (StoreId == 0)
                {
                    return "";
                }
                Logger.Trace("Settings:"+StoreSettings.Count());
                var item = StoreSettings.FirstOrDefault(r => r.SettingKey.Equals(key, StringComparison.InvariantCultureIgnoreCase));

                return item != null ? item.SettingValue : "";
            }
            catch (Exception ex)
            {

                Logger.Error(ex, "Store= " + StoreId + " Key=" + key, key);
                return "";
            }
        }

    }
}