using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.Entities
{
    public class Location :BaseEntity
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string LocationState { get; set; }
        public string Postal { get; set; }
        public string Country { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
