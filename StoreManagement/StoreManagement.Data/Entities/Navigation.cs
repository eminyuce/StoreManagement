using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using GenericRepository;
using Newtonsoft.Json;

namespace StoreManagement.Data.Entities
{
 
    public class Navigation : BaseEntity
    {
        public int ParentId { get; set; }
        [Required(ErrorMessage = "Please enter name")]
        public new string Name { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public Boolean Static { get; set; }
        public string Link { get; set; }



         [JsonIgnore]
        public string Modul { get; set; }
         [JsonIgnore]
        public Boolean LinkState { get; set; }
     
    }
}
