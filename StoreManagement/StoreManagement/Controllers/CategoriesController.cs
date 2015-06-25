using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
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
    public class CategoriesController : BaseController
    {
         

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

            Task<List<Category>> task1 = CategoryService.GetCategoriesByStoreIdAsync(Store.Id, StoreConstants.BlogsType);
            var task2 = ContentService.GetContentsCategoryIdAsync(Store.Id, categoryId, StoreConstants.BlogsType, true, page, 600);
            var task3 = CategoryService.GetCategoryAsync(categoryId);
            await Task.WhenAll(task1, task2, task3);

            returnModel.Categories = task1.Result;
            returnModel.Store = Store;
            returnModel.Category = task3.Result;

            returnModel.Contents = new PagedList<Content>(task2.Result.items, task2.Result.page - 1, task2.Result.pageSize, task2.Result.totalItemCount);

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

            var task1 = CategoryService.GetCategoriesByStoreId(Store.Id, StoreConstants.BlogsType);
            var task2 = ContentService.GetContentsCategoryId(Store.Id, categoryId, StoreConstants.BlogsType, true, page, 600);
            var task3 = CategoryService.GetCategory(categoryId);
            //await Task.WhenAll(task1, task2, task3);

            returnModel.Categories = task1;
            returnModel.Store = Store;
            returnModel.Category = task3;

            returnModel.Contents = new PagedList<Content>(task2.items, task2.page - 1, task2.pageSize, task2.totalItemCount);

            // Stop timing
            stopwatch.Stop();
            Logger.Info("Sync Time elapsed 3: {0}", stopwatch.ElapsedMilliseconds);
            return View("Category",returnModel);

        }
    }
}