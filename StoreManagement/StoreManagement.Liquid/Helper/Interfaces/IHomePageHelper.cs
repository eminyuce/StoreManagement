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
        StoreLiquidResult GetHomePageDesign(
            Task<List<Product>> productsTask,
            Task<List<Content>> blogsTask,
            Task<List<Content>> newsTask,
            Task<List<FileManager>> sliderTask,
            Task<PageDesign> pageDesignTask,
            Task<List<Category>> categoriesTask,
            Task<List<ProductCategory>> productCategoriesTask);

        int StoreId { get; set; }
    }

}