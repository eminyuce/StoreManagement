using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcPaging;
using NLog;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.Services.IServices;

namespace StoreManagement.Service.Services
{
    public class ProductCategoryService : BaseService, IProductCategoryService 
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        public ProductCategoryViewModel GetProductCategory(string id, int page)
        {
            var resultModel = new ProductCategoryViewModel();
            int categoryId = id.Split("-".ToCharArray()).Last().ToInt();
            resultModel.SCategories = ProductCategoryRepository.GetProductCategoriesByStoreId(MyStore.Id, StoreConstants.ProductType);
            resultModel.SStore = MyStore;
            resultModel.SCategory = ProductCategoryRepository.GetProductCategory(categoryId);
            var m = ProductRepository.GetProductsCategoryId(MyStore.Id, categoryId, StoreConstants.ProductType, true, page, 24);
            resultModel.SProducts = new PagedList<Product>(m.items, m.page - 1, m.pageSize, m.totalItemCount);
            resultModel.SNavigations = NavigationRepository.GetStoreActiveNavigations(this.MyStore.Id);
            resultModel.SSettings = this.GetStoreSettings();
            return resultModel;

        }
    }
}
