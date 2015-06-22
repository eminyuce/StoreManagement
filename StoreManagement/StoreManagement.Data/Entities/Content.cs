using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using GenericRepository;

namespace StoreManagement.Data.Entities
{
    public class Content : BaseContent
    {
        public Content()
        {

        }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public virtual ICollection<ContentFile> ContentFiles { get; set; }

    }
}
