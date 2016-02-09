using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using MvcPaging;
using NLog;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.LiquidEngineHelpers;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.LiquidHelpers;
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

        public StoreLiquidResult GetCategoriesIndexPage(PageDesign pageDesign, StorePagedList<Category> categories, string type)
        {
            var result = new StoreLiquidResult();

            try
            {



                var cats = new List<CategoryLiquid>();
                foreach (var item in categories.items)
                {
                    cats.Add(new CategoryLiquid(item, type));
                }

                object anonymousObject = new
                {
                    categories = LiquidAnonymousObject.GetCategoriesLiquid(cats)
                };

                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign, anonymousObject);


                var dic = new Dictionary<String, String>();
                dic.Add(StoreConstants.PageOutput, indexPageOutput);
                dic.Add(StoreConstants.PageSize, categories.pageSize.ToStr());
                dic.Add(StoreConstants.PageNumber, categories.page.ToStr());
                dic.Add(StoreConstants.TotalItemCount, categories.totalItemCount.ToStr());
                //dic.Add(StoreConstants.IsPagingUp, pageDesign.IsPagingUp ? Boolean.TrueString : Boolean.FalseString);
                //dic.Add(StoreConstants.IsPagingDown, pageDesign.IsPagingDown ? Boolean.TrueString : Boolean.FalseString);

                result.LiquidRenderedResult = dic;
                result.PageDesingName = pageDesign.Name;
            }
            catch (Exception exception)
            {
                Logger.Error(exception, "GetCategoriesIndexPage : categories and pageDesign", String.Format("Categories Items Count : {0}", categories.items.Count));
            }

            return result;

        }

        public StoreLiquidResult GetCategoryPage(PageDesign pageDesign, Category category, String type)
        {
            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, "");
            try
            {

                var contentCategory = new CategoryLiquid(category, type);

                object anonymousObject = new
                {
                    category = LiquidAnonymousObject.GetCategory(contentCategory)
                };

                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign, anonymousObject);
                dic[StoreConstants.PageOutput] = indexPageOutput;


            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetCategoriesPartial");
            }

            var result = new StoreLiquidResult();
            result.LiquidRenderedResult = dic;
            result.PageDesingName = pageDesign.Name;
            return result;
        }
    }
}
