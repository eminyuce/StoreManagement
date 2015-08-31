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
    public class ContactHelper : BaseLiquidHelper, IContactHelper
    {
        public StoreLiquidResult GetContactIndexPage(Task<PageDesign> pageDesignTask)
        {
            var result = new StoreLiquidResult();
            var dic = new Dictionary<String, String>();
            result.LiquidRenderedResult = dic;
            dic.Add(StoreConstants.PageOutput, "");

            try
            {


                Task.WaitAll(pageDesignTask);
                var pageDesign = pageDesignTask.Result;


                if (pageDesign == null)
                {
                    throw new Exception("PageDesing is null");
                }



                object anonymousObject = new
                {

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