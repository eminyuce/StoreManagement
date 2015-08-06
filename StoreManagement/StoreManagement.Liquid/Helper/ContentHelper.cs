using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.LiquidEngineHelpers;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.Paging;

namespace StoreManagement.Liquid.Helper
{
    public class ContentHelper
    {
        public static Dictionary<string, string> GetContentsIndexPage(
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
                    var blog = new ContentLiquid(item, category, pageDesign, type);
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
                            s.DetailLink,
                            s.ImageHas,
                            s.ImageSource
                        }
            }
                );


            var dic = new Dictionary<String, String>();
            dic.Add("PageOutput", indexPageOutput);
            dic.Add("PageSize", contents.pageSize.ToStr());
            dic.Add("PageNumber", (contents.page - 1).ToStr());
            dic.Add("TotalItemCount", contents.totalItemCount.ToStr());
            dic.Add("IsPagingUp", pageDesign.IsPagingUp ? Boolean.TrueString : Boolean.FalseString);
            dic.Add("IsPagingDown", pageDesign.IsPagingDown ? Boolean.TrueString : Boolean.FalseString);


            return dic;
        }


        public static Dictionary<string, string> GetContentDetailPage(Task<Content> contentTask, Task<PageDesign> pageDesignTask, Task<Category> categoryTask, String type)
        {
            Task.WaitAll(pageDesignTask, contentTask, categoryTask);
            var content = contentTask.Result;
            var pageDesign = pageDesignTask.Result;
            var category = categoryTask.Result;
            var items = new List<ContentLiquid>();
            var contentLiquid = new ContentLiquid(content, category, pageDesign, type);

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
            dic.Add("PageOutput", indexPageOutput);


            return dic;
        }

        public static Dictionary<string, string> GetRelatedContentsPartial(Task<Category> categoryTask, Task<List<Content>> relatedContentsTask, Task<PageDesign> pageDesignTask, String type)
        {
            Task.WaitAll(pageDesignTask, relatedContentsTask, categoryTask);
            var contents = relatedContentsTask.Result;
            var pageDesign = pageDesignTask.Result;
            var category = categoryTask.Result;

            var items = new List<ContentLiquid>();
            foreach (var item in contents)
            {
                var blog = new ContentLiquid(item, category, pageDesign, type);
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
                            s.ImageHas,
                            s.ImageSource
                        }
            }
                );


            var dic = new Dictionary<String, String>();
            dic.Add("PageOutput", indexPageOutput);


            return dic;

        }
    }
}