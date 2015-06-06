using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcPaging;
using StoreManagement.Data.Entities;

namespace StoreManagement.Data.RequestModel
{
    public class StoreHomePage
    {
        public Store Store { get; set; }  
        public List<Category> Categories  { get; set; }
        public List<StoreCarousel> CarouselImages { get; set; }
        public PagedList<Content> Blogs { get; set; }
        public PagedList<Content> News { get; set; }
        public PagedList<Content> Products { get; set; }
    }
}
