using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.Paging;

namespace StoreManagement.Liquid.Helper.Interfaces
{
    public interface ICategoryHelper : IHelper
    {
        StoreLiquidResult GetCategoriesIndexPage(PageDesign pageDesign, StorePagedList<Category> categories, String type);

        StoreLiquidResult GetCategoryPage(PageDesign pageDesign, Category category, StorePagedList<Content> contents, String type);
    }

   
}