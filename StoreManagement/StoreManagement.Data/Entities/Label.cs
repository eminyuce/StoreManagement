using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.Entities
{
      
    public class Label : BaseEntity
    {
        [Required(ErrorMessage = "Please enter name")]
        public String Name { get; set; }
        public int ParentId { get; set; }


        public override string ToString()
        {
            return "id:" + this.Id + " Name:" + Name + " StoreId:" + this.StoreId;
        }
    }
}
