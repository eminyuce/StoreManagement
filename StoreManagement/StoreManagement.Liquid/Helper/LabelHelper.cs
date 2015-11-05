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
   

    public class LabelHelper : BaseLiquidHelper, ILabelHelper
    {


        public StoreLiquidResult GetProductLabels(
           List<Label> labels,
           PageDesign pageDesign)
        {
            

            var items = new List<LabelLiquid>();
            foreach (var item in labels)
            {

                var nav = new LabelLiquid(item);
                items.Add(nav);
            }


            object anonymousObject = new
            {
                labels = LiquidAnonymousObject.GetLabelsEnumerable(items)


            };


            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign, anonymousObject);


            var dic = new Dictionary<String, String>();

            dic.Add(StoreConstants.PageOutput, indexPageOutput);


            var result = new StoreLiquidResult();
            result.PageDesingName = pageDesign.Name;
            result.LiquidRenderedResult = dic;
            return result;
        }

       
    }
}