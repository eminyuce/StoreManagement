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
    public class Product : BaseContent
    {

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please enter category")]
        public int ProductCategoryId { get; set; }

        public float Price { get; set; }
        public float Discount { get; set; }
        public int BrandId { get; set; }
        public int ProductCode { get; set; }


        public virtual ICollection<ProductFile> ProductFiles { get; set; }

    }
}
