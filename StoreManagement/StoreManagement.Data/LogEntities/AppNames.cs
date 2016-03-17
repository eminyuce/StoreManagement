using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.LogEntities
{
    public class AppNames
    {
        public string Log_application { get; set; }

        public DateTime LastActivity { get; set; }

        public DateTime LastError { get; set; }


        public int TotalRecords { get; set; }
    }
}
