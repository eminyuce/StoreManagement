using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.LiquidEngineHelpers;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.LiquidHelpers.Interfaces;

namespace StoreManagement.Data.LiquidHelpers
{
   

    public class NavigationHelper : BaseLiquidHelper, INavigationHelper
    {

        public StoreLiquidResult GetMainLayoutLink(
          List<Navigation> navigations,
          PageDesign pageDesign)
        {
            

            if (pageDesign == null)
            {
                throw new Exception("PageDesing is null");
            }


            var items = new List<NavigationLiquid>();
            foreach (var item in navigations)
            {

                var nav = new NavigationLiquid(item);
                items.Add(nav);
            }


            object anonymousObject = new
                {
                    navigations = LiquidAnonymousObject.GetNavigationsEnumerable(items)
                    

                };


            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign, anonymousObject);


            var dic = new Dictionary<String, String>();
 
            dic.Add(StoreConstants.PageOutput, indexPageOutput);


            var result = new StoreLiquidResult();
            result.LiquidRenderedResult = dic;
            result.PageDesingName = pageDesign.Name;
            return result;
        }


        public StoreLiquidResult GetMainLayoutFooterLink(List<Navigation> navigations, PageDesign pageDesign)
        {
          

            var items = new List<NavigationLiquid>();
            foreach (var item in navigations)
            {

                var nav = new NavigationLiquid(item);
                items.Add(nav);
            }


            object anonymousObject = new
            {
                items = LiquidAnonymousObject.GetNavigationsEnumerable(items)


            };


            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign, anonymousObject);


            var dic = new Dictionary<String, String>();
 
            dic.Add(StoreConstants.PageOutput, indexPageOutput);



            var result = new StoreLiquidResult();
            result.LiquidRenderedResult = dic;
            result.PageDesingName = pageDesign.Name;
            return result;
        }

      
    }
}