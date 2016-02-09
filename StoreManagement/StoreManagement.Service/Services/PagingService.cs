using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using NLog;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.LiquidEngineHelpers;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Service.Services.IServices;

namespace StoreManagement.Service.Services
{
    public class PagingService : BaseService, IPagingService
    {

        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public StoreLiquidResult PageOutput { get; set; }


        public String ActionName { get; set; }
        public string ControllerName { get; set; }
        public HttpRequestBase HttpRequestBase { get; set; }
        public RouteData RouteData { get; set; }
        public String PaginatePath
        {
            get
            {
                var rv = new Dictionary<String, String>();
                string id = RouteData.Values["id"].ToStr();
                string filters = RouteData.Values["filters"].ToStr();
                if (!String.IsNullOrEmpty(id))
                {
                    rv.Add("id", id);
                }
                if (!String.IsNullOrEmpty(filters))
                {
                    rv.Add("filters", filters);
                }
                foreach (var key in HttpRequestBase.QueryString.AllKeys)
                {

                    if (!String.IsNullOrEmpty(key) && key.ToLower() != "page")
                    {
                        if (!rv.ContainsKey(key))
                        {
                            rv.Add(key, HttpRequestBase.QueryString[key]);
                        }
                    }
                }

                String queryString = "";
                queryString += rv.ContainsKey("id") ? "/" + rv["id"] : "";
                queryString += rv.ContainsKey("filters") ? "/" + rv["filters"] : "";
                for (int i = rv.Count - 1; i >= 0; i--)
                {
                    var item = rv.ElementAt(i);
                    var itemKey = item.Key;
                    var itemValue = item.Value;

                    if (itemKey.Equals("id", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //queryString += "/" + rv[itemKey];
                    }
                    else if (itemKey.Equals("filters", StringComparison.InvariantCultureIgnoreCase))
                    {
                        // queryString += "/" + rv[itemKey];
                    }
                    else
                    {
                        queryString += (i == 0 ? "?" : "&") + itemKey + "=" + itemValue;
                    }
                }



                String m = rv.ContainsKey("id") || rv.ContainsKey("filters") ? "?" : "&";

                return String.Format("/{2}/{0}{1}page=:num", ActionName.Equals("index", StringComparison.InvariantCultureIgnoreCase) ? "" : ActionName, String.IsNullOrEmpty(queryString) ? "?" : queryString + m, ControllerName).ToLower();
            }
        }

        public StoreLiquidResult GetPaging(PageDesign pageDesign)
        {




            var paginator = new PaginatorLiquid();
            paginator.PaginatePath = this.PaginatePath;
            var pageOutputDictionary = PageOutput.LiquidRenderedResult;
            if (pageOutputDictionary.ContainsKey(StoreConstants.PageNumber))
            {
                paginator.Page = pageOutputDictionary[StoreConstants.PageNumber].ToInt();
            }
            else
            {
                Logger.Error("Key NOT FOUND :" + StoreConstants.PageNumber);
            }

            if (pageOutputDictionary.ContainsKey(StoreConstants.TotalItemCount))
            {
                paginator.TotalRecords = pageOutputDictionary[StoreConstants.TotalItemCount].ToInt();
            }
            else
            {
                Logger.Error("Key NOT FOUND :" + StoreConstants.TotalItemCount);
            }

            if (pageOutputDictionary.ContainsKey(StoreConstants.PageSize))
            {
                paginator.PageSize = pageOutputDictionary[StoreConstants.PageSize].ToInt();
            }
            else
            {
                Logger.Error("Key NOT FOUND :" + StoreConstants.PageSize);
            }


            object anonymousObject = new
            {
                paginator = paginator
            };
            var pagingHtml = LiquidEngineHelper.RenderPage(pageDesign, anonymousObject);



            //var pagingDic = new Dictionary<String, String>();
            //pagingDic.Add(StoreConstants.PagingOutput, indexPageOutput);
            //pagingDic = pagingDic.MergeLeft(pageOutputDictionary);
            String html = "";
            if (pageOutputDictionary.ContainsKey(StoreConstants.PageOutput))
            {
                html = pageOutputDictionary[StoreConstants.PageOutput];
            }
            else
            {
                Logger.Error("Key NOT FOUND :" + StoreConstants.PageOutput);
            }


            pageOutputDictionary[StoreConstants.PageOutput] = HtmlAttributeHelper.AddPaging(html, pagingHtml);

            //PageOutput.LiquidRenderedResult = pagingDic;
            return PageOutput;
        }

    }
}
