using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;
using GenericRepository;

namespace StoreManagement.Data.Entities
{
        [Serializable]
    public abstract class BaseEntity : IEntity 
    {

        public int Id { get; set; }
        public int StoreId { get; set; }
        public DateTime ? CreatedDate { get; set; }
        public DateTime ? UpdatedDate { get; set; }

        [IgnoreDataMember]
        public bool State { get; set; }
        [IgnoreDataMember]
        public int Ordering { get; set; }



        public override string ToString()
        {
            return "id:" + this.Id + " StoreId:" + this.StoreId;
        }

        
    }
}
