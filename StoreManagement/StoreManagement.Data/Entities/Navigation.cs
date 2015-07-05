using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;

namespace StoreManagement.Data.Entities
{
    public class Navigation : BaseEntity
    {
        public int ParentId { get; set; }
        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public Boolean Static { get; set; }
        public string Link { get; set; }



        [IgnoreDataMember]
        public string Modul { get; set; }
        [IgnoreDataMember]
        public Boolean LinkState { get; set; }
     
    }
}
