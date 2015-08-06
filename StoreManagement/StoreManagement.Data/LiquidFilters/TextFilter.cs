using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace StoreManagement.Data.LiquidFilters
{
    public static class TextFilter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static string Textilize(string input)
        {
            Logger.Info("TextFilder method is called."+input);
            return input + "emin";
        }
    }
}
