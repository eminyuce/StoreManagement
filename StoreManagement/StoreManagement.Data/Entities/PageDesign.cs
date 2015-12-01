using GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace StoreManagement.Data.Entities
{

    public class PageDesign : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }


        public bool State { get; set; }
        [JsonIgnore]
        public int Ordering { get; set; }


        public string Type { get; set; }

        public int StorePageDesignId { get; set; }

        [AllowHtml]
        public string PageTemplate { get; set; }

        public override string ToString()
        {
            return String.Format(
            "Id:{0} Type:{1} PageRazorTemplate:{2} StoreId:{3} ", Id, Type, PageTemplate, Name);
        }
    }

}
