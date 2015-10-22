using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.Entities
{
    public class Retailer:BaseEntity
    {
        public String RetailerCode { get; set; }
        public String RetailerUrl { get; set; }
    }
}
