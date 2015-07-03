using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.Entities
{
    public class Label : BaseEntity
    {
        public String Name { get; set; }
        public int ParentId { get; set; }
    }
}
