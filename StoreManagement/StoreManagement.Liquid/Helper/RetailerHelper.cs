using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEngineHelpers;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Liquid.Helper.Interfaces;

namespace StoreManagement.Liquid.Helper
{
    public class RetailerHelper : BaseLiquidHelper, IRetailerHelper
    {
        public StoreLiquidResult GetRetailers(List<Retailer> labels,
                                       PageDesign pageDesign)
        {


            var items = new List<RetailerLiquid>();
            foreach (var item in labels)
            {

                var nav = new RetailerLiquid(item);
                items.Add(nav);
            }


            object anonymousObject = new
            {
                labels = LiquidAnonymousObject.GetRetailersEnumerable(items)


            };


            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);


            var dic = new Dictionary<String, String>();

            dic.Add(StoreConstants.PageOutput, indexPageOutput);


            var result = new StoreLiquidResult();
            result.PageDesingName = pageDesign.Name;
            result.LiquidRenderedResult = dic;
            return result;
        }

    }
}