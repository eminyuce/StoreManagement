using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DotLiquid;
using GenericRepository;

namespace StoreManagement.Data.Entities
{

          [Serializable]
    public class Content : BaseContent
    {
        public Content()
        {

        }


        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please enter category")]
        public int CategoryId { get; set; }
        public String Author { get; set; }


        public virtual ICollection<ContentFile> ContentFiles { get; set; }




    }
}
