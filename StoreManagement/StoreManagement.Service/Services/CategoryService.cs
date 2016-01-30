using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using MvcPaging;
using NLog;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.Paging;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.Services.IServices;

namespace StoreManagement.Service.Services
{
    public class CategoryService : BaseService ,  ICategoryService
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public CategoryViewModel GetCategory(string id, int page, String categoryType)
        {
            var returnModel = new CategoryViewModel();
            int categoryId = id.Split("-".ToCharArray()).Last().ToInt();

            StorePagedList<Content> task2 = ContentRepository.GetContentsCategoryId(MyStore.Id, categoryId, categoryType, true, page, 600);


            returnModel.SCategories = CategoryRepository.GetCategoriesByStoreId(MyStore.Id, categoryType, true);
            returnModel.SStore = MyStore;
            returnModel.SCategory = CategoryRepository.GetCategory(categoryId);
            returnModel.Type = categoryType;
            returnModel.SNavigations = NavigationRepository.GetStoreActiveNavigations(this.MyStore.Id);
            returnModel.SContents = new PagedList<Content>(task2.items, task2.page - 1, task2.pageSize, task2.totalItemCount);


            return returnModel;
        }
    }
}
