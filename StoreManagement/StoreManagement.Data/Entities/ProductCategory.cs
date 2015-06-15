using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.Entities
{
    public class ProductCategory : BaseEntity
    {
        public int ParentId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }

        [IgnoreDataMember]
        public bool State { get; set; }
        [IgnoreDataMember]
        public int Ordering { get; set; }
        [IgnoreDataMember]
        public string CategoryType { get; set; }

        public virtual ICollection<Product> Products { get; set; }





    }
}
