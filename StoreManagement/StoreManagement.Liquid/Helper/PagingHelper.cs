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
using StoreManagement.Liquid.Helper.Interfaces;

namespace StoreManagement.Liquid.Helper
{
    
    public class PagingHelper : BaseLiquidHelper, IPagingHelper
    {

        public StoreLiquidResult PageOutput { get; set; }

 
        public String ActionName { get; set; }
        public string ControllerName { get; set; }
        public String PaginatePath
        {
            get
            {
                if (ActionName.Equals("Index", StringComparison.InvariantCultureIgnoreCase))
                {
                    return String.Format("?page=:num");
                }
                else
                {
                    return String.Format("{0}?page=:num", ActionName);
                }
            }
        }

        public StoreLiquidResult GetPaging(Task<PageDesign> pageDesignTask)
        {
            Task.WaitAll(pageDesignTask);
            var pageDesign = pageDesignTask.Result;

            if (pageDesign == null)
            {
                throw new Exception("PageDesing is null");
            }


            var paginator = new PaginatorLiquid();
            paginator.PaginatePath = this.PaginatePath;
            var pageOutputDictionary = PageOutput.LiquidRenderedResult;
            paginator.Page = pageOutputDictionary[StoreConstants.PageNumber].ToInt();
            paginator.TotalRecords = pageOutputDictionary[StoreConstants.TotalItemCount].ToInt();
            paginator.PageSize = pageOutputDictionary[StoreConstants.PageSize].ToInt();
            object anonymousObject = new
                {
                    paginator = paginator
                };
            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);



            var pagingDic = new Dictionary<String, String>();
            pagingDic.Add(StoreConstants.PagingOutput, indexPageOutput);
            pagingDic = pagingDic.MergeLeft(pageOutputDictionary);
            PageOutput.LiquidRenderedResult = pagingDic;
            return PageOutput;
        }




    }
}