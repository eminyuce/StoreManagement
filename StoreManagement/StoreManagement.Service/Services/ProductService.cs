using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using StoreManagement.Data.Constants;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.IGeneralRepositories;
using StoreManagement.Service.Services.IServices;

namespace StoreManagement.Service.Services
{
    public class ProductService : BaseService, IProductService 
    {
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
    }
}
