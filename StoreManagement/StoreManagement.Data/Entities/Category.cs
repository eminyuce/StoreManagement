using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;

namespace StoreManagement.Data.Entities
{
    public class Category : BaseCategory
    {
        public virtual ICollection<Content> Contents { get; set; }
    }
}
