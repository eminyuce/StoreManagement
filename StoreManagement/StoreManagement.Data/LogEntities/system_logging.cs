using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;

namespace StoreManagement.Data.LogEntities
{
    public class system_logging : IEntity
    {
        public Guid system_logging_guid { get; set; }
        public int Id { get; set; }
        public DateTime entered_date { get; set; }
        public string log_application { get; set; }
        public string log_date { get; set; }
        public string log_level { get; set; }
        public string log_logger { get; set; }
        public string log_message { get; set; }
        public string log_machine_name { get; set; }
        public string log_user_name { get; set; }
        public string log_call_site { get; set; }
        public string log_thread { get; set; }
        public string log_exception { get; set; }
        public string log_stacktrace { get; set; }
    }
}
