using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcPaging;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Data.RequestModel
{
    public class StoreHomePage : ViewModel
    {
        public List<ProductCategory> SProductCategories { get; set; }
        public List<Category> SNewsCategories { get; set; }
        public List<Category> SBlogsCategories { get; set; }

        public List<FileManager> SCarouselImages { get; set; }
        public PagedList<Product> SProducts { get; set; }
        public PagedList<Content> SNews { get; set; }
        public PagedList<Content> SBlogs { get; set; }



        public List<ProductCategoryLiquid> ProductCategories
        {
            get { return SProductCategories.Select(r => new ProductCategoryLiquid(r)).ToList(); }
        }

        public List<ProductLiquid> Products
        {
            get { return SProducts.Select(r => new ProductLiquid(r, this.SProductCategories.FirstOrDefault(r2 => r2.Id == r.ProductCategoryId))).ToList(); }
        }


        public List<CategoryLiquid> NewsCategories
        {
            get { return SNewsCategories.Select(r => new CategoryLiquid(r, StoreConstants.NewsType)).ToList(); }
        }

        public List<ContentLiquid> News
        {
            get { return SNews.Select(r => new ContentLiquid(r, this.SNewsCategories.FirstOrDefault(r2 => r2.Id == r.CategoryId), StoreConstants.NewsType)).ToList(); }
        }

        public List<CategoryLiquid> BlogsCategories
        {
            get { return SBlogsCategories.Select(r => new CategoryLiquid(r, StoreConstants.BlogsType)).ToList(); }
        }

        public List<ContentLiquid> Blogs
        {
            get { return SBlogs.Select(r => new ContentLiquid(r, this.SBlogsCategories.FirstOrDefault(r2 => r2.Id == r.CategoryId), StoreConstants.BlogsType)).ToList(); }
        }

      
    }
}
