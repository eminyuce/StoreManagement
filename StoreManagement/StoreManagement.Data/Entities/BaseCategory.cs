using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace StoreManagement.Data.Entities
{
  
    public class BaseCategory : BaseEntity
    {
        public int ParentId { get; set; }
        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }        
        public string Description { get; set; }
        [ScriptIgnore]
        public string CategoryType { get; set; }
       
    }
}
