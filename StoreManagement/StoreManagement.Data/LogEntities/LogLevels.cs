using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.LogEntities
{
    public enum LogLevels
    {
        [Description("Fatal")]
        Fatal = 20,
        [Description("Error")]
        Error = 30,
        [Description("Warn")]
        Warn = 40,
        [Description("Info")]
        Info = 50,
        [Description("Debug")]
        Debug = 60,
        [Description("Trace")]
        Trace = 70



    }
}
