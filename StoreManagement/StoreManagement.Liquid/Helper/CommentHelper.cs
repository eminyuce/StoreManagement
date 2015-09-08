using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEngineHelpers;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Liquid.Helper.Interfaces;

namespace StoreManagement.Liquid.Helper
{
    public class CommentHelper : BaseLiquidHelper, ICommentHelper
    {
        public StoreLiquidResult GetCommentsPartial(Task<List<Comment>> commentsTask, Task<PageDesign> pageDesignTask)
        {
            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, "");
            try
            {
                Task.WaitAll(pageDesignTask, commentsTask);
                var pageDesign = pageDesignTask.Result;
                var comments = commentsTask.Result;

                if (pageDesign == null)
                {
                    throw new Exception("PageDesing is null");
                }


                var items = new List<CommentLiquid>();
                foreach (var item in comments)
                {

                    var c = new CommentLiquid(item, pageDesign, ImageWidth, ImageHeight);
                    items.Add(c);

                }


                object anonymousObject = new
                {
                    comments = LiquidAnonymousObject.GetCommentsEnumerable(items)
                };

                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);


                dic[StoreConstants.PageOutput] = indexPageOutput;

            }
            catch (Exception ex)
            {
                Logger.Error(ex);

            }


            var result = new StoreLiquidResult();
            result.LiquidRenderedResult = dic;
            return result;
        }
    }
}