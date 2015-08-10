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
    public class PagingHelper : BaseLiquidHelper
    {

        public Dictionary<string, string> PageOutputDictionary { get; set; }

 
        public String ActionName { get; set; }
        public string ControllerName { get; set; }
        public String PaginatePath
        {
            get
            {
                if (ActionName.Equals("Index", StringComparison.InvariantCultureIgnoreCase))
                {
                    return String.Format("{0}?page=:num", ControllerName);
                }
                else
                {
                    return String.Format("{0}/{1}?page=:num", ControllerName, ActionName);
                }
            }
        }

        public Dictionary<string, string> GetPaging(Task<PageDesign> pageDesignTask)
        {
            Task.WaitAll(pageDesignTask);
            var pageDesign = pageDesignTask.Result;

            var paginator = new PaginatorLiquid();
            paginator.PaginatePath = this.PaginatePath;
    
            paginator.Page = PageOutputDictionary[StoreConstants.PageNumber].ToInt();
            paginator.TotalRecords = PageOutputDictionary[StoreConstants.TotalItemCount].ToInt();
            paginator.PageSize = PageOutputDictionary[StoreConstants.PageSize].ToInt();
            object anonymousObject = new
                {
                    paginator = paginator
                };
            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);



            var pagingDic = new Dictionary<String, String>();
            pagingDic.Add(StoreConstants.PagingOutput, indexPageOutput);
            pagingDic = pagingDic.MergeLeft(PageOutputDictionary);

            return pagingDic;
        }




    }
}