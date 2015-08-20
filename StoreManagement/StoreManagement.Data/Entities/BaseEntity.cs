using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using DotLiquid;
using GenericRepository;
using Newtonsoft.Json;

namespace StoreManagement.Data.Entities
{

    public abstract class BaseEntity : IEntity
    {

        public int Id { get; set; }
        public int StoreId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

 
        public bool State { get; set; }
        [JsonIgnore]
        public int Ordering { get; set; }



        public override string ToString()
        {
            return "id:" + this.Id + " StoreId:" + this.StoreId;
        }


    }
}
