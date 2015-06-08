using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.GeneralHelper
{
    public static class EnumHelper
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
                return (T)EnumHelper.Parse<T>(value, ignoreCase);
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
    }
}
