using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StoreManagement.Data.Entities
{
    public abstract class BaseContent : BaseEntity 
    {
         [Required]
        public string Name { get; set; }

        [AllowHtml]
        public string Description { get; set; }

        [IgnoreDataMember]
        public string Type { get; set; }
        [IgnoreDataMember]
        public Boolean MainPage { get; set; }
        [IgnoreDataMember]
        public Boolean ImageState { get; set; }
    }
}
