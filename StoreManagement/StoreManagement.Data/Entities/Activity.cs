using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.Entities
{
    public class Activity : BaseEntity
    {
        public string Description { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime FinishDate { get; set; }
    }
}
