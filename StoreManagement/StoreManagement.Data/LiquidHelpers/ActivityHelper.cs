using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEngineHelpers;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.LiquidHelpers.Interfaces;

namespace StoreManagement.Data.LiquidHelpers
{
    public class ActivityHelper : BaseLiquidHelper, IActivityHelper
    {
        public StoreLiquidResult GetActivityIndexPage(PageDesign pageDesign, List<Activity> activities)
        {
            var result = new StoreLiquidResult();
            result.PageDesingName = pageDesign.Name;
            var dic = new Dictionary<String, String>();
            result.LiquidRenderedResult = dic;
            dic.Add(StoreConstants.PageOutput, "");

            try
            {


            

                var items = new List<ActivitiesLiquid>();
                foreach (var item in activities)
                {

                    var i = new ActivitiesLiquid(item,  ImageWidth, ImageHeight);
                    items.Add(i);

                }


                object anonymousObject = new
                {
                    activities = LiquidAnonymousObject.GetActivitiesEnumerable(items)


                };


                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign, anonymousObject);


                dic[StoreConstants.PageOutput] = indexPageOutput;


            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return result;
        }

    }
}