using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.Entities
{
    public class ProductAttribute : BaseEntity
    {
        public bool IsFilterable { get; set; }
        public bool IsRequired { get; set; }
        public bool IsSearchable { get; set; }
        public String ComponentType { get; set; }
        public String DefaultValue { get; set; }
    }
}
