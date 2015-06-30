using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.HelpersModel;

namespace StoreManagement.Data.LogEntities
{
    public class LoggingModel
    {
        public RecordsStats RecordsStats { get; set; }

        private List<system_logging> _systemLogging = new List<system_logging>();
        public List<system_logging> SystemLoggingList
        {
            get { return _systemLogging; }
            set { _systemLogging = value; }
        }
        private List<String> _allApplicationNames = new List<String>();
        public List<String> AllApplicationNames
        {
            get { return _allApplicationNames; }
            set { _allApplicationNames = value; }
        }


        public string ApplicationName { get; set; }
    }
}
