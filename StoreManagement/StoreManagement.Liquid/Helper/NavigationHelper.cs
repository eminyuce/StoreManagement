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

namespace StoreManagement.Liquid.Helper
{
    public class NavigationHelper : BaseLiquidHelper
    {

        public StoreLiquidResult GetMainLayoutLink(
           Task<List<Navigation>> navigationsTask,
           Task<PageDesign> pageDesignTask)
        {
            Task.WaitAll(pageDesignTask, navigationsTask);
            var navigations = navigationsTask.Result;
            var pageDesign = pageDesignTask.Result;

            var items = new List<NavigationLiquid>();
            foreach (var item in navigations)
            {

                var nav = new NavigationLiquid(item, pageDesign);
                items.Add(nav);
            }


            object anonymousObject = new
                {
                    items = from s in items
                            select new
                                {
                                    s.Navigation.Name,
                                    s.Link
                                }
                    

                };


            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);


            var dic = new Dictionary<String, String>();
 
            dic.Add(StoreConstants.PageOutput, indexPageOutput);


            var result = new StoreLiquidResult();
            result.LiquidRenderedResult = dic;
            return result;
        }


        public static StoreLiquidResult GetMainLayoutFooterLink(Task<List<Navigation>> navigationsTask, Task<PageDesign> pageDesignTask)
        {
            Task.WaitAll(pageDesignTask, navigationsTask);
            var navigations = navigationsTask.Result;
            var pageDesign = pageDesignTask.Result;

            var items = new List<NavigationLiquid>();
            foreach (var item in navigations)
            {

                var nav = new NavigationLiquid(item, pageDesign);
                items.Add(nav);
            }


            object anonymousObject = new
            {
                items = from s in items
                        select new
                        {
                            s.Navigation.Name,
                            s.Link
                        }


            };


            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);


            var dic = new Dictionary<String, String>();
 
            dic.Add(StoreConstants.PageOutput, indexPageOutput);



            var result = new StoreLiquidResult();
            result.LiquidRenderedResult = dic;
            return result;
        }
    }
}