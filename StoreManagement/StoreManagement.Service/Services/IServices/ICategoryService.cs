using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.Paging;
using StoreManagement.Data.RequestModel;

namespace StoreManagement.Service.Services.IServices
{
    public interface ICategoryService : IBaseService
    {
         CategoryViewModel GetCategory(string id, int page, String categoryType);
         StoreLiquidResult GetCategoriesIndexPage(PageDesign pageDesign, StorePagedList<Category> categories, String type);

         StoreLiquidResult GetCategoryPage(PageDesign pageDesign, Category category, String type);
    }
}
