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
        public StoreLiquidResult GetCommentsPartial(List<Comment> comments, PageDesign pageDesign)
        {
            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, "");
            try
            {
           

                if (pageDesign == null)
                {
                    throw new Exception("PageDesing is null");
                }


                var items = new List<CommentLiquid>();
                foreach (var item in comments)
                {

                    var c = new CommentLiquid(item,  ImageWidth, ImageHeight);
                    items.Add(c);

                }


                object anonymousObject = new
                {
                    comments = LiquidAnonymousObject.GetCommentsEnumerable(items)
                };

                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign, anonymousObject);


                dic[StoreConstants.PageOutput] = indexPageOutput;

            }
            catch (Exception ex)
            {
                Logger.Error(ex);

            }


            var result = new StoreLiquidResult();
            result.LiquidRenderedResult = dic;
            result.PageDesingName = pageDesign.Name;
            return result;
        }
    }
}