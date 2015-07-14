using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StoreManagement.Data.Entities
{
    public class Brand : BaseEntity
    {
        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }
        [AllowHtml]
        public string Description { get; set; }


        public override string ToString()
        {
            return "id:" + this.Id + " Name:" + Name + " StoreId:" + this.StoreId;
        }

    }
}
