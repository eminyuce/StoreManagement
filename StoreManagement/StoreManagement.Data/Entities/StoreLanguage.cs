using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StoreManagement.Data.Entities
{
    public class StoreLanguage : BaseEntity
    {
  
        public string LanguageCode { get; set; }
    }
}
