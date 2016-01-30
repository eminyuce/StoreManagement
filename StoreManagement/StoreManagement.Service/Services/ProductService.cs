using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcPaging;
using NLog;
using Ninject;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.IGeneralRepositories;
using StoreManagement.Service.Repositories;
using StoreManagement.Service.Services.IServices;

namespace StoreManagement.Service.Services
{
    public class ProductService : BaseService, IProductService 
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public ProductDetailViewModel GetProductDetailPage(string id)
        {
            var resultModel = new ProductDetailViewModel();
            int productId = id.Split("-".ToCharArray()).Last().ToInt();
            resultModel.SProduct = ProductRepository.GetProductsById(productId);
            resultModel.SStore = MyStore;
            resultModel.SCategory = ProductCategoryRepository.GetProductCategory(resultModel.Product.ProductCategoryId);
            resultModel.SCategories = ProductCategoryRepository.GetProductCategoriesByStoreId(MyStore.Id, StoreConstants.ProductType);
            resultModel.SNavigations = NavigationRepository.GetStoreActiveNavigations(this.MyStore.Id);
            resultModel.SSettings = this.GetStoreSettings();
            return resultModel;
        }

        public ProductsViewModel GetProductIndexPage(string search, string page)
        {
            var resultModel = new ProductsViewModel();
            resultModel.SCategories = ProductCategoryRepository.GetProductCategoriesByStoreIdFromCache(MyStore.Id, StoreConstants.ProductType);
            resultModel.SStore = MyStore;
            resultModel.SNavigations = NavigationRepository.GetStoreActiveNavigations(this.MyStore.Id);
            resultModel.SSettings = this.GetStoreSettings();
            return resultModel;
        }

        public StoreHomePage GetHomePage()
        {
            int page = 1;
            StoreHomePage resultModel = new StoreHomePage();

                resultModel.SStore = MyStore;
                resultModel.SCarouselImages = FileManagerRepository.GetStoreCarousels(MyStore.Id);
                resultModel.SProductCategories = ProductCategoryRepository.GetProductCategoriesByStoreId(MyStore.Id);
                var products = ProductRepository.GetProductsCategoryId(MyStore.Id, null, StoreConstants.ProductType, true, page, 24);
                resultModel.SProducts = new PagedList<Product>(products.items, products.page - 1, products.pageSize, products.totalItemCount);
                var contents = ContentRepository.GetContentsCategoryId(MyStore.Id, null, StoreConstants.NewsType, true, page, 24);
                resultModel.SNews = new PagedList<Content>(contents.items, contents.page - 1, contents.pageSize, contents.totalItemCount);
                contents = ContentRepository.GetContentsCategoryId(MyStore.Id, null, StoreConstants.BlogsType, true, page, 24);
                resultModel.SBlogs = new PagedList<Content>(contents.items, contents.page - 1, contents.pageSize, contents.totalItemCount);
                resultModel.SBlogsCategories = CategoryRepository.GetCategoriesByStoreId(MyStore.Id, StoreConstants.BlogsType, true);
                resultModel.SNewsCategories = CategoryRepository.GetCategoriesByStoreId(MyStore.Id, StoreConstants.NewsType, true);
                resultModel.SNavigations = NavigationRepository.GetStoreActiveNavigations(this.MyStore.Id);
                resultModel.SSettings = this.GetStoreSettings();


            return resultModel;
        }
    }
}
