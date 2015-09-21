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
using Newtonsoft.Json;

namespace StoreManagement.Data.Entities
{

     public abstract class BaseContent : BaseEntity 
    {
         //[Required]
        //public new string Name { get; set; }

        [AllowHtml]
        public string Description { get; set; }

         [JsonIgnore]
        public string Type { get; set; }
         [JsonIgnore]
        public Boolean MainPage { get; set; }
 
        public Boolean ImageState { get; set; }

        public int TotalRating { get; set; }
         [AllowHtml]
         public string VideoUrl { get; set; }


        public override string ToString()
        {
            return "id:" + this.Id + " Name:" + Name + " StoreId:" + this.StoreId;
        }
    }
}
