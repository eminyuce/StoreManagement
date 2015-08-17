using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DotLiquid;

namespace StoreManagement.Data.Entities
{

     public abstract class BaseContent : BaseEntity 
    {
         [Required]
 
        public string Name { get; set; }

        [AllowHtml]
        public string Description { get; set; }

        [ScriptIgnore]
        public string Type { get; set; }
        [ScriptIgnore]
        public Boolean MainPage { get; set; }
        [ScriptIgnore]
        public Boolean ImageState { get; set; }


        public override string ToString()
        {
            return "id:" + this.Id + " Name:" + Name + " StoreId:" + this.StoreId;
        }
    }
}
