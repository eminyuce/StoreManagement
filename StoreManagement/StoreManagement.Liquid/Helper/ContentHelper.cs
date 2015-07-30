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
                 Task<List<Category>> categoriesTask)
        {
            Task.WaitAll(pageDesignTask, contentsTask, categoriesTask);
            var contents = contentsTask.Result;
            var blogsPageDesign = pageDesignTask.Result;
            var categories = categoriesTask.Result;
            var items = new List<ContentLiquid>();
            foreach (var item in contents.items)
            {
                var category = categories.FirstOrDefault(r => r.Id == item.CategoryId);
                if (category != null)
                {
                    var blog = new ContentLiquid(item, category, blogsPageDesign);
                    items.Add(blog);
                }
            }

            var indexPageOutput = LiquidEngineHelper.RenderPage(blogsPageDesign.PageTemplate, new
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
            dic.Add("IsPagingUp", blogsPageDesign.IsPagingUp ? Boolean.TrueString : Boolean.FalseString);
            dic.Add("IsPagingDown", blogsPageDesign.IsPagingDown ? Boolean.TrueString : Boolean.FalseString);


            return dic;
        }


    }
}