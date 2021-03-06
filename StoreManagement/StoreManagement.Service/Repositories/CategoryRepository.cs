﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;
using GenericRepository.EntityFramework.Enums;
using StoreManagement.Data.Entities;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.HelpersModel;
using StoreManagement.Data.Paging;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.GenericRepositories;
using StoreManagement.Service.Repositories.Interfaces;
using StoreManagement.Data;

namespace StoreManagement.Service.Repositories
{
    public class CategoryRepository : BaseRepository<Category, int>, ICategoryRepository
    {

        private static readonly TypedObjectCache<List<Category>> CategoryCache
            = new TypedObjectCache<List<Category>>("categoryCache");

        private static readonly TypedObjectCache<StorePagedList<Category>> PagingCategoryCache
         = new TypedObjectCache<StorePagedList<Category>>("PagingCategoryCache");


        public CategoryRepository(IStoreContext dbContext)
            : base(dbContext)
        {

        }

        public List<Category> GetCategoriesByStoreIdWithContent(int storeId, int? take)
        {
            var query = StoreDbContext.Categories.Where(r => r.StoreId == storeId && r.Contents.Any()).Include(r => r.Contents.Select(r1 => r1.ContentFiles.Select(m => m.FileManager)));

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.OrderByDescending(r => r.Ordering).ToList();
        }


        private static Expression<Func<Category, object>> IncludeProperties()
        {
            var param = Expression.Parameter(typeof(Category));
            Expression<Func<Category, object>> exp = r => r.Contents.Select(m => m.ContentFiles.Select(p => p.FileManager));

            return exp;
        }

        public List<Category> GetCategoriesByStoreId(int storeId, String type, bool? isActive)
        {
            return BaseCategoryRepository.GetCategoriesByStoreId(this, storeId, type, isActive);
        }

        public List<Category> GetCategoriesByStoreId(int storeId, string type, string search)
        {
            return BaseCategoryRepository.GetBaseCategoriesSearchList(this, storeId, search, type);
        }

        public List<Category> GetCategoriesByType(string type)
        {
            return this.FindBy(r => r.CategoryType.Equals(type, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        public List<Category> GetCategoriesByStoreIdFromCache(int storeId, String type)
        {
            String key = String.Format("GetCategoriesByStoreIdFromCache-{0}-{1}", storeId, type);
            List<Category> items = null;
            CategoryCache.TryGet(key, out items);
            if (items == null)
            {

                var cats = this.FindBy(r => r.StoreId == storeId &&
                         r.CategoryType.Equals(type, StringComparison.InvariantCultureIgnoreCase))
                         .OrderBy(r => r.Ordering)
                         .Include(r => r.Contents.Select(r1 => r1.ContentFiles.Select(m => m.FileManager)));

                items = cats.ToList();

                foreach (var category in items)
                {
                    foreach (var ccc in category.Contents)
                    {
                        ccc.Description = ""; // GeneralHelper.GetDescription(ccc.Description, 200);
                    }
                }


                CategoryCache.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("Categories_CacheAbsoluteExpiration_Minute", 10)));
            }

            return items;
        }

        public Category GetCategory(int id)
        {
            return this.GetSingle(id);
        }



        public async Task<List<Category>> GetCategoriesByStoreIdAsync(int storeId, string type, bool? isActive)
        {
            return await BaseCategoryRepository.GetCategoriesByStoreIdAsync(this, storeId, type, isActive, null);
        }

        public async Task<Category> GetCategoryAsync(int id)
        {
            return await this.GetSingleAsync(id);
        }

        public async Task<Category> GetCategoryByContentIdAsync(int storeId, int contentId)
        {
            Content category = this.DbContext.Contents.FirstOrDefault(r => r.Id == contentId);
            if (category != null)
            {
                return await this.GetSingleAsync(category.CategoryId);
            }
            else
            {
                return null;
            }
        }

        public async Task<StorePagedList<Category>> GetCategoriesByStoreIdWithPagingAsync(int storeId, string type, bool? isActive, int page = 1, int pageSize = 25)
        {
            Expression<Func<Category, bool>> match = r2 => r2.StoreId == storeId && r2.State == (isActive.HasValue ? isActive.Value : r2.State) & r2.CategoryType.Equals(type, StringComparison.InvariantCultureIgnoreCase);
            Expression<Func<Category, int>> keySelector = t => t.Ordering;

            var items = await this.FindAllAsync(match, keySelector, OrderByType.Descending, page, pageSize);
            var totalItemNumber = await this.CountAsync(match);


            var task = Task.Factory.StartNew(() =>
            {
                StorePagedList<Category> resultItems = new StorePagedList<Category>(items, page, pageSize, totalItemNumber);
                return resultItems;
            });
            var result = await task;

            return result;
        }

        public List<Category> GetCategoriesByStoreId(int storeId)
        {
            var categories = from entry in this.FindBy(r => r.StoreId == storeId) select entry;
            return categories.Where(r => r.State).OrderBy(r => r.Ordering).ToList();


        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
