using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.Entities
{
    public class BaseCategory : BaseEntity
    {
        public int ParentId { get; set; }
        public string Name { get; set; }
        [IgnoreDataMember]
        public string CategoryType { get; set; }
       
    }
}
