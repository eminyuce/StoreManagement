using System;
using System.Collections;
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
    public class LocationHelper : BaseLiquidHelper, ILocationHelper
    {


        public StoreLiquidResult GetLocationIndexPage(Task<PageDesign> pageDesignTask, Task<List<Location>> locationsTask)
        {
            var result = new StoreLiquidResult();
            var dic = new Dictionary<String, String>();
            result.LiquidRenderedResult = dic;
            dic.Add(StoreConstants.PageOutput, "");

            try
            {


                Task.WaitAll(pageDesignTask, locationsTask);
                var pageDesign = pageDesignTask.Result;
                var locations = locationsTask.Result;

                if (pageDesign == null)
                {
                    throw new Exception("PageDesing is null");
                }


                var items = new List<LocationLiquid>();
                foreach (var item in locations)
                {

                    var i = new LocationLiquid(item, pageDesign, ImageWidth, ImageHeight);
                    items.Add(i);

                }


                object anonymousObject = new
                {
                    items = LiquidAnonymousObject.GetLocationsEnumerable(items)


                };


                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);


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