using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.Entities
{
    public class ProductCategory : BaseCategory
    {
        public virtual ICollection<Product> Products { get; set; }


    }
}
