using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcPaging;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Data.RequestModel
{
    public class StoreHomePage : BaseDrop
    {
        public Store Store { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
        public List<FileManager> CarouselImages { get; set; }


        public PagedList<Product> Products { get; set; }

        public PagedList<Content> News { get; set; }

        public PagedList<Content> Blogs { get; set; }
    }
}
