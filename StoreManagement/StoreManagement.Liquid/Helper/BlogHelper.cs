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
    public class BlogHelper
    {
        public static Dictionary<string, string> GetBlogsIndexPage(HttpRequestBase httpRequestBase,
            Task<StorePagedList<Content>> contentsTask,
            Task<PageDesign> blogsPageDesignTask,
                 Task<List<Category>> categoriesTask)
        {
            var contents = contentsTask.Result;
            var blogsPageDesign = blogsPageDesignTask.Result;
            var categories = categoriesTask.Result;
            var blogs = new List<Blog>();
            foreach (var item in contents.items)
            {
                var category = categories.FirstOrDefault(r => r.Id == item.CategoryId);
                if (category != null)
                {
                    var blog = new Blog(httpRequestBase, item, category);
                    blogs.Add(blog);
                }
            }

            var indexPageOutput = LiquidEngineHelper.RenderPage(blogsPageDesign.PageTemplate, new
            {
                blogs = from s in blogs
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
            dic.Add("BlogsIndex", indexPageOutput);
            dic.Add("PageSize", contents.pageSize.ToStr());
            dic.Add("PageNumber", (contents.page - 1).ToStr());
            dic.Add("TotalItemCount", contents.totalItemCount.ToStr());
            return dic;
        }

    }
}