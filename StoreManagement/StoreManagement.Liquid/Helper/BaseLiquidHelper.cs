using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;
using Ninject;
using StoreManagement.Data;
using StoreManagement.Data.EmailHelper;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Liquid.Helper.Interfaces;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Liquid.Helper
{
    public abstract class BaseLiquidHelper : IHelper
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

                var item = StoreSettings.FirstOrDefault(r => r.SettingKey.RemoveTabNewLines().Equals(key.RemoveTabNewLines(), StringComparison.InvariantCultureIgnoreCase));

                return item != null ? item.SettingValue : "";
            }
            catch (Exception ex)
            {

                Logger.Error(ex, "Store= " + StoreId + " Key=" + key, key);
                return "";
            }
        }

        protected IEnumerable GetProductsLiquid(List<ProductLiquid> productLiquidList)
        {
            return from s in productLiquidList
                   select new
                   {
                       CategoryName = s.Category.Name,
                       ProductCategoryId = s.Product.ProductCategoryId,
                       BrandId = s.Product.BrandId,
                       CategoryDescription = s.Category.Description,
                       ProductId = s.Product.Id,
                       s.Product.Name,
                       s.Product.Description,
                       s.Product.ProductCode,
                       s.Product.Price,
                       s.Product.Discount,
                       s.Product.UpdatedDate,
                       s.Product.CreatedDate,
                       s.Product.TotalRating,
                       s.Product.UnitsInStock,
                       s.DetailLink,
                       images = s.ImageLiquid

                   };
        }

        protected IEnumerable GetContentLiquid(List<ContentLiquid> blogsLiquidList)
        {
            return from s in blogsLiquidList
                   select new
                   {
                       s.Content.Name,
                       s.Content.Description,
                       s.Content.Author,
                       s.Content.UpdatedDate,
                       s.DetailLink,
                       images = s.ImageLiquid

                   };
        }

    }
}