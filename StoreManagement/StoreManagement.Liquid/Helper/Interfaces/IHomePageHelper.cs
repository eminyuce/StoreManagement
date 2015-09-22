using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Liquid.Helper.Interfaces
{
    public interface IHomePageHelper : IHelper
    {
       

        StoreLiquidResult GetHomePageDesign(PageDesign pageDesing, List<FileManager> sliderImages,
                                                   List<Product> products, List<Content> blogs, List<Content> news,
                                                   List<Category> categories, List<ProductCategory> productCategories);


        StoreLiquidResult GetHomePageDesign(PageDesign pageDesing, List<FileManager> sliderImages);
    }

}