using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.Entities
{
       
    public class Contact : BaseEntity
    {
//[Required(ErrorMessage = "Please enter name")]
      //  public string Name { get; set; }
        [Required(ErrorMessage = "Please enter title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public string PhoneWork { get; set; }
        public string PhoneCell { get; set; }
        public string Fax { get; set; }


        public override string ToString()
        {
            return "id:" + this.Id + " Name:" + Name + " StoreId:" + this.StoreId;
        }
    }
}
