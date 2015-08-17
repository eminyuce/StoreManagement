using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.Entities
{
     
    public class Location : BaseEntity
    {
        [Required(ErrorMessage = "Please enter address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Please enter city")]
        public string City { get; set; }
        public string LocationState { get; set; }
        [Required(ErrorMessage = "Please  enter postal")]
        public string Postal { get; set; }
        [Required(ErrorMessage = "Please enter country")]
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
