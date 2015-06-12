using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;

namespace StoreManagement.Data.Entities
{
    public abstract class BaseEntity: IEntity
    {
        public int Id { get; set; }
        [IgnoreDataMember]
        public int StoreId { get; set; }
    }
}
