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
    public class StoreHomePage : BaseDrop
    {
        public Store Store { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
        public List<Category> NewsCategories { get; set; }
        public List<Category> BlogsCategories { get; set; }

        public List<FileManager> CarouselImages { get; set; }
        public PagedList<Product> Products { get; set; }
        public PagedList<Content> News { get; set; }
        public PagedList<Content> Blogs { get; set; }



        public List<ProductCategoryLiquid> ProductCategoryLiquids
        {
            get { return ProductCategories.Select(r => new ProductCategoryLiquid(r)).ToList(); }
        }

        public List<ProductLiquid> ProductLiquids
        {
            get { return Products.Select(r => new ProductLiquid(r, this.ProductCategories.FirstOrDefault(r2 => r2.Id == r.ProductCategoryId))).ToList(); }
        }


        public List<CategoryLiquid> NewsCategoryLiquids
        {
            get { return NewsCategories.Select(r => new CategoryLiquid(r, StoreConstants.NewsType)).ToList(); }
        }

        public List<ContentLiquid> NewsContentLiquids
        {
            get { return News.Select(r => new ContentLiquid(r, this.NewsCategories.FirstOrDefault(r2 => r2.Id == r.CategoryId), StoreConstants.NewsType)).ToList(); }
        }

        public List<CategoryLiquid> BlogsCategoryLiquids
        {
            get { return BlogsCategories.Select(r => new CategoryLiquid(r, StoreConstants.BlogsType)).ToList(); }
        }

        public List<ContentLiquid> BlogsContentLiquids
        {
            get { return Blogs.Select(r => new ContentLiquid(r, this.NewsCategories.FirstOrDefault(r2 => r2.Id == r.CategoryId), StoreConstants.BlogsType)).ToList(); }
        }

        public StoreLiquid StoreLiquid
        {
            get
            {
                return new StoreLiquid(Store);
            }
        }

    }
}
