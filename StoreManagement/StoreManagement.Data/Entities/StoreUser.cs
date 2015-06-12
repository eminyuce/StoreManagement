using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;

namespace StoreManagement.Data.Entities
{
    public class StoreUser : IEntity
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int UserId { get; set; }

        public virtual UserProfile UserProfile { get; set; }

    }
}
