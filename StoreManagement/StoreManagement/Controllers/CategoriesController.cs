using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using NLog;
using Ninject;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Controllers
{
    [OutputCache(CacheProfile = "Cache1Days")]
    public class CategoriesController : BaseController
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> Category(String id, int page = 1)
        {
            // Create new stopwatch
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing
            stopwatch.Start();

           
    

            var returnModel = new CategoryViewModel();
            int categoryId = id.Split("-".ToCharArray()).Last().ToInt();

            Task<List<Category>> task1 = CategoryService.GetCategoriesByStoreIdAsync(MyStore.Id, StoreConstants.BlogsType,null);
            var task2 = ContentService.GetContentsCategoryIdAsync(MyStore.Id, categoryId, StoreConstants.BlogsType, true, page, 600,"");
            var task3 = CategoryService.GetCategoryAsync(categoryId);
            await Task.WhenAll(task1, task2, task3);

            returnModel.SCategories = task1.Result;
            returnModel.SStore = MyStore;
            returnModel.SCategory = task3.Result;
            returnModel.SNavigations = NavigationService.GetStoreActiveNavigations(this.MyStore.Id);
            returnModel.SContents = new PagedList<Content>(task2.Result.items, task2.Result.page - 1, task2.Result.pageSize, task2.Result.totalItemCount);

            // Stop timing
            stopwatch.Stop();
            Logger.Info("Async Time elapsed 3: {0}", stopwatch.ElapsedMilliseconds);
            return View(returnModel);

        }
        public ActionResult Category2(String id, int page = 1)
        {
            // Create new stopwatch
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing
            stopwatch.Start();




            var returnModel = new CategoryViewModel();
            int categoryId = id.Split("-".ToCharArray()).Last().ToInt();

            var task1 = CategoryService.GetCategoriesByStoreId(MyStore.Id, StoreConstants.BlogsType);
            var task2 = ContentService.GetContentsCategoryId(MyStore.Id, categoryId, StoreConstants.BlogsType, true, page, 600);
            var task3 = CategoryService.GetCategory(categoryId);
            //await Task.WhenAll(task1, task2, task3);

            returnModel.SCategories = task1;
            returnModel.SStore = MyStore;
            returnModel.SCategory = task3;
            returnModel.SNavigations = NavigationService.GetStoreActiveNavigations(this.MyStore.Id);
            returnModel.SContents = new PagedList<Content>(task2.items, task2.page - 1, task2.pageSize, task2.totalItemCount);

            // Stop timing
            stopwatch.Stop();
            Logger.Info("Sync Time elapsed 3: {0}", stopwatch.ElapsedMilliseconds);
            return View("Category",returnModel);

        }
    }
}