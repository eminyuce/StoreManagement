﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.LiquidEngineHelpers;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.Paging;
using StoreManagement.Liquid.Helper.Interfaces;

namespace StoreManagement.Liquid.Helper
{

    public class PagingHelper : BaseLiquidHelper, IPagingHelper
    {

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
                if (!String.IsNullOrEmpty(id))
                {
                    rv.Add("id", id);
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
                var idExists = rv.Keys.Count == 1 && rv.ContainsKey("id");
                for (int i = rv.Count - 1; i >= 0; i--)
                {
                    var item = rv.ElementAt(i);
                    var itemKey = item.Key;
                    var itemValue = item.Value;

                    if (itemKey.Equals("id", StringComparison.InvariantCultureIgnoreCase))
                    {
                        queryString += "/" + rv[itemKey];
                    }
                    else
                    {
                        queryString += (i == 0 ? "?" : "&") + itemKey + "=" + itemValue;
                    }
                }

                //if (ActionName.Equals("Index", StringComparison.InvariantCultureIgnoreCase))
                //{

                //    //return String.Format("{0}page=:num", String.IsNullOrEmpty(queryString) ? "?" : queryString + "&");
                //    return String.Format("/{2}/{0}{1}page=:num", ActionName, String.IsNullOrEmpty(queryString) ? "?" : queryString + m, ControllerName);
                //}
                //else
                //{

                //}


                String m = idExists ? "?" : "&";
                return String.Format("/{2}/{0}{1}page=:num", ActionName, String.IsNullOrEmpty(queryString) ? "?" : queryString + m, ControllerName);
            }
        }

        public StoreLiquidResult GetPaging(Task<PageDesign> pageDesignTask)
        {
            Task.WaitAll(pageDesignTask);
            var pageDesign = pageDesignTask.Result;

            if (pageDesign == null)
            {
                throw new Exception("PageDesing is null");
            }


            var paginator = new PaginatorLiquid();
            paginator.PaginatePath = this.PaginatePath;
            var pageOutputDictionary = PageOutput.LiquidRenderedResult;
            paginator.Page = pageOutputDictionary[StoreConstants.PageNumber].ToInt();
            paginator.TotalRecords = pageOutputDictionary[StoreConstants.TotalItemCount].ToInt();
            paginator.PageSize = pageOutputDictionary[StoreConstants.PageSize].ToInt();
            object anonymousObject = new
                {
                    paginator = paginator
                };
            var pagingHtml = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);



            //var pagingDic = new Dictionary<String, String>();
            //pagingDic.Add(StoreConstants.PagingOutput, indexPageOutput);
            //pagingDic = pagingDic.MergeLeft(pageOutputDictionary);
            var html = pageOutputDictionary[StoreConstants.PageOutput];
            pageOutputDictionary[StoreConstants.PageOutput] = HtmlAttributeHelper.AddPaging(html, pagingHtml);

            //PageOutput.LiquidRenderedResult = pagingDic;
            return PageOutput;
        }




    }
}

