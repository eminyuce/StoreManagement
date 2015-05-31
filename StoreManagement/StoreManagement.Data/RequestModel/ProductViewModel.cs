using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Data.RequestModel
{
    public class ProductViewModel
    {
        public Store Store { get; set; }
        public Category Category { get; set; }
        public Content Content { get; set; }
        public List<Category> Categories { get; set; }
        public List<Content> RelatedContents { get; set; }
    }
}
