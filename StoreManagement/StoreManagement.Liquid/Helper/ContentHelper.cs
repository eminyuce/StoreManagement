using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.LiquidEngineHelpers;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.Paging;

namespace StoreManagement.Liquid.Helper
{
    public class ContentHelper : BaseLiquidHelper
    {

  


        public Dictionary<string, string> GetContentsIndexPage(
            Task<StorePagedList<Content>> contentsTask,
            Task<PageDesign> pageDesignTask,
                 Task<List<Category>> categoriesTask, String type)
        {
            Task.WaitAll(pageDesignTask, contentsTask, categoriesTask);
            var contents = contentsTask.Result;
            var pageDesign = pageDesignTask.Result;
            var categories = categoriesTask.Result;
            var items = new List<ContentLiquid>();
            foreach (var item in contents.items)
            {
                var category = categories.FirstOrDefault(r => r.Id == item.CategoryId);
                if (category != null)
                {
                    var blog = new ContentLiquid(item, category, pageDesign, type, ImageWidth, ImageHeight);
                    items.Add(blog);
                }
            }

            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, new
            {
                items = from s in items
                        select new
                        {
                            s.Content.Name,
                            s.Content.Description,
                            s.Content.Author,
                            s.Content.UpdatedDate,
                            s.DetailLink,
                            s.ImageLiquid.ImageHas,
                            s.ImageLiquid.ImageSource
                        }
            }
                );


            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, indexPageOutput);
            dic.Add(StoreConstants.PageSize, contents.pageSize.ToStr());
            dic.Add(StoreConstants.PageNumber, contents.page.ToStr());
            dic.Add(StoreConstants.TotalItemCount, contents.totalItemCount.ToStr());
            dic.Add(StoreConstants.IsPagingUp, pageDesign.IsPagingUp ? Boolean.TrueString : Boolean.FalseString);
            dic.Add(StoreConstants.IsPagingDown, pageDesign.IsPagingDown ? Boolean.TrueString : Boolean.FalseString);

            return dic;
        }


        public Dictionary<string, string> GetContentDetailPage(Task<Content> contentTask, Task<PageDesign> pageDesignTask, Task<Category> categoryTask, String type)
        {
            Task.WaitAll(pageDesignTask, contentTask, categoryTask);
            var content = contentTask.Result;
            var pageDesign = pageDesignTask.Result;
            var category = categoryTask.Result;
            var items = new List<ContentLiquid>();
            var contentLiquid = new ContentLiquid(content, category, pageDesign, type, ImageWidth, ImageHeight);

            object anonymousObject = new
            {
                CategoryId = contentLiquid.Content.CategoryId,
                CategoryName = contentLiquid.Category.Name,
                CategoryDescription = contentLiquid.Category.Description,
                ContentId = contentLiquid.Content.Id,
                Name = contentLiquid.Content.Name,
                Description = contentLiquid.Content.Description,
                ImageSource = contentLiquid.ImageLiquid.ImageSource,
                Images = contentLiquid.ImageLiquid.ImageLinks
            };

            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);



            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, indexPageOutput);


            return dic;
        }

        public Dictionary<string, string> GetRelatedContentsPartial(Task<Category> categoryTask, Task<List<Content>> relatedContentsTask, Task<PageDesign> pageDesignTask, String type)
        {
            Task.WaitAll(pageDesignTask, relatedContentsTask, categoryTask);
            var contents = relatedContentsTask.Result;
            var pageDesign = pageDesignTask.Result;
            var category = categoryTask.Result;

            var items = new List<ContentLiquid>();
            foreach (var item in contents)
            {
                var blog = new ContentLiquid(item, category, pageDesign, type, ImageWidth, ImageHeight);
                items.Add(blog);

            }

            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, new
            {
                items = from s in items
                        select new
                        {
                            s.Content.Name,
                            s.Content.Description,
                            s.DetailLink,
                            s.ImageLiquid.ImageHas,
                            s.ImageLiquid.ImageSource
                        }
            }
                );


            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, indexPageOutput);


            return dic;

        }
    }
}