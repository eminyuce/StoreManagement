using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;

namespace StoreManagement.Data.Entities
{
    public class Navigation : BaseEntity
    {
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string ControllerName { get; set; }
        public Boolean Static { get; set; }
        public string Link { get; set; }
        public DateTime CreatedDate { get; set; }



        [IgnoreDataMember]
        public string Modul { get; set; }
        [IgnoreDataMember]
        public int Ordering { get; set; }
        [IgnoreDataMember]
        public Boolean LinkState { get; set; }
        [IgnoreDataMember]
        public Boolean State { get; set; }
    }
}
