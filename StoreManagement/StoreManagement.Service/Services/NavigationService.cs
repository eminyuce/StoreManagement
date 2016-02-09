using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEngineHelpers;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Service.Services.IServices;

namespace StoreManagement.Service.Services
{
    public class NavigationService : BaseService, INavigationService
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public bool IsModulActive(string controllerName)
        {
            var navigations = NavigationRepository.GetStoreActiveNavigations(MyStore.Id);
           return navigations.Any(r => r.ControllerName.ToLower().StartsWith(controllerName.ToLower()));
        }

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
