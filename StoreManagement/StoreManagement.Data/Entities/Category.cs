using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;

namespace StoreManagement.Data.Entities
{
    public class Category : IEntity 
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int ParentId { get; set; }
        public int Ordering { get; set; }
        public string CategoryType { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Content> Contents { get; set; }
    }
}
