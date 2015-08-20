using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace StoreManagement.Data.GeneralHelper
{
    public static class DictionaryExtensions
    {

        static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Returns the default value of type U if the key does not exist in the dictionary
        /// </summary>
        public static U GetOrDefault<T, U>(this Dictionary<T, U> dic, T key)
        {
            if (dic.ContainsKey(key))
            {
                return dic[key];
            }
            else
            {
                Logger.Error("Dictionary does not have any value for " + key);
                return default(U);
            }
        }

        /// <summary>
        /// Returns an existing value U for key T, or creates a new instance of type U using the default constructor, 
        /// adds it to the dictionary and returns it.
        /// </summary>
        public static U GetOrInsertNew<T, U>(this Dictionary<T, U> dic, T key)
            where U : new()
        {
            if (dic.ContainsKey(key)) return dic[key];
            U newObj = new U();
            dic[key] = newObj;
            return newObj;
        }
    }
}
