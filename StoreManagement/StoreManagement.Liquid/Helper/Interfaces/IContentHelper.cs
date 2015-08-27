using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.Paging;

namespace StoreManagement.Liquid.Helper.Interfaces
{
    public interface IContentHelper : IHelper
    {
        StoreLiquidResult GetContentsIndexPage(
            Task<StorePagedList<Content>> contentsTask,
            Task<PageDesign> pageDesignTask,
            Task<List<Category>> categoriesTask, String type);

        StoreLiquidResult GetContentDetailPage(Task<Content> contentTask, Task<PageDesign> pageDesignTask, Task<Category> categoryTask, String type);
        StoreLiquidResult GetRelatedContentsPartial(Task<Category> categoryTask, Task<List<Content>> relatedContentsTask, Task<PageDesign> pageDesignTask, String type);
    }
}