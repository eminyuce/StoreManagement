using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Data.LiquidEntities
{
    public class HomePageLiquid
    {
        public PageDesign PageDesing;
        public List<FileManager> SliderImages;
        public List<Content> Blogs { get; set; }
        public List<Product> Products { get; set; }
        public List<Content> News { get; set; }

        public HomePageLiquid(PageDesign pageDesing, List<FileManager> sliderImages)
        {
            // TODO: Complete member initialization
            this.PageDesing = pageDesing;
            this.SliderImages = sliderImages;
        }

    
    }
}
