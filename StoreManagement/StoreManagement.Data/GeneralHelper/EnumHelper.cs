using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StoreManagement.Data.GeneralHelper
{

    public static class YuceEnumHelper
    {
        public static Nullable<T> Parse<T>(String value, Boolean ignoreCase) where T : struct
        {
            return String.IsNullOrEmpty(value) ? null : (Nullable<T>)Enum.Parse(typeof(T), value, ignoreCase);
        }

        public static T[] GetValues<T>()
        {
            return (T[])Enum.GetValues(typeof(T));
        }
        public static string EnumToString<T>(object value)
        {
            return Enum.GetName(typeof(T), value);
        }

        public static T Parse<T>(String value, Boolean ignoreCase, T defaultEnum) where T : struct
        {
            if ((!string.IsNullOrEmpty(value)) && (Enum.IsDefined(typeof(T), value)))
                return (T)YuceEnumHelper.Parse<T>(value, ignoreCase);
            else
                return defaultEnum;
        }

        public static string GetEnumDescription(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return en.ToString();
        }

        public static IEnumerable<SelectListItem> ToSelectList(this Enum enumValue)
        {
            return from Enum e in Enum.GetValues(enumValue.GetType())
                   select new SelectListItem
                   {
                       Selected = e.Equals(enumValue),
                       Text = e.ToDescription(),
                       Value = e.ToString()
                   };
        }

        public static string ToDescription(this Enum value)
        {
            var attributes = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static IEnumerable<SelectListItem> ToSelectListWithId(this Enum enumValue)
        {
            return from Enum e in Enum.GetValues(enumValue.GetType())
                   select new SelectListItem
                   {
                       Selected = e.Equals(enumValue),
                       Text = e.ToDescription(),
                       Value = Convert.ToInt32(e).ToStr()
                   };
        }
    }
}
