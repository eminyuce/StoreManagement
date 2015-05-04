using StoreManagement.Data.HelpersModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Filter = StoreManagement.Data.HelpersModel.Filter;

namespace StoreManagement.Data.GeneralHelper
{
    public class FilterHelper
    {
        public static List<Filter> ParseFiltersFromString(string filters)
        {
            var items = new List<Filter>();

            if (!string.IsNullOrEmpty(filters))
            {
                var stringFilters = filters.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);




                foreach (var stringFilter in stringFilters)
                {


                    if (stringFilter.IndexOf('-') > 0)
                    {
                        var filter = new Filter();
                        var firstHyphen = stringFilter.IndexOf('-');
                        filter.FieldName = stringFilter.Substring(0, firstHyphen).UrlDecode();


                        if (stringFilter.IndexOf('-', firstHyphen + 1) > 0)
                        {
                            var secondHyphen = stringFilter.IndexOf('-', firstHyphen + 1);
                            filter.ValueFirst =
                                stringFilter.Substring(firstHyphen + 1, secondHyphen - firstHyphen - 1).UrlDecode();
                            filter.ValueLast =
                                stringFilter.Substring(secondHyphen + 1, stringFilter.Length - secondHyphen - 1).
                                    UrlDecode();

                        }
                        else
                        {
                            filter.ValueFirst =
                                stringFilter.Substring(firstHyphen + 1, stringFilter.Length - firstHyphen - 1).UrlDecode
                                    ();
                           


                        }

                        items.Add(filter);
                    }//if
                }//for each
            }//end if
            return items;

        }

        public static string PageLink(HttpRequestBase httpRequestBase, ViewContext viewContext, int page, ItemType itemType)
        {

            string sFilters = (string)viewContext.RouteData.Values["filters"];
            var filters = FilterHelper.ParseFiltersFromString(sFilters);



            var rv = new RouteValueDictionary();

            if (filters != null)
            {
                string urlFilters = string.Join("/",
                                         filters.OrderBy(i => i.FieldName).Select(
                                             i => i.Url));
                rv.Add("filters", urlFilters);
            }



            foreach (var key in httpRequestBase.QueryString.AllKeys)
            {
                if (key.ToLower() != "page")
                {
                    rv.Add(key, httpRequestBase.QueryString[key]);
                }
            }

            if (page > 1)
            {
                rv.Add("page", page.ToString());
            }

            var urlHelper = new UrlHelper(httpRequestBase.RequestContext);
            return urlHelper.Action(itemType.SearchAction, itemType.Controller, rv);
        }

        public static List<Filter> GetNavigationFilters(ViewContext viewContext)
        {
            string filters = (string)viewContext.RouteData.Values["filters"];
            var fltrs = FilterHelper.ParseFiltersFromString(filters);
            return fltrs.OrderBy(i => i.FieldName).ToList();
        }

        public static string Link(Filter f,HttpRequestBase httpRequestBase, ViewContext viewContext)
        {

            string sFilters = (string)viewContext.RouteData.Values["filters"];
            var filters = FilterHelper.ParseFiltersFromString(sFilters);

            string urlFilters;

            if (filters != null)
            {
                if (!filters.Any(i => i.FieldName.ToLower() == f.FieldName.ToLower()))
                {
                    filters.Add(f);
                }

                urlFilters = string.Join("/",
                                         filters.OrderBy(i => i.FieldName).Select(
                                             i => (i.FieldName.ToLower() == f.FieldName.ToLower()) ? f.Url : i.Url));
            }
            else
            {
                urlFilters = f.Url;
            }



            var rv = new RouteValueDictionary();
            rv.Add("filters", urlFilters);

            foreach (var key in httpRequestBase.QueryString.AllKeys)
            {
                if (key.ToLower() != "page")
                {
                    rv.Add(key, httpRequestBase.QueryString[key]);
                }
            }

            var urlHelper = new UrlHelper(httpRequestBase.RequestContext);
            return urlHelper.Action(f.OwnerType.SearchAction, f.OwnerType.Controller, rv);
        }





        public static string LinkExclude(Filter f, HttpRequestBase httpRequestBase, ViewContext viewContext, ItemType ownerType)
        {
            //RequestContext
            string sFilters = (string)viewContext.RouteData.Values["filters"];
            var filters = FilterHelper.ParseFiltersFromString(sFilters);


            var rv = new RouteValueDictionary();


            if (filters != null)
            {
                int index = filters.FindIndex(i => i.FieldName.ToLower() == f.FieldName.ToLower());
                if (index >= 0)
                {
                    filters.RemoveAt(index);
                }

                string urlFilters = string.Join("/",
                                        filters.OrderBy(i => i.FieldName).Select(
                                            i => (i.FieldName.ToLower() == f.FieldName.ToLower()) ? f.Url : i.Url));

                rv.Add("filters", urlFilters);
            }




            foreach (var key in httpRequestBase.QueryString.AllKeys)
            {
                if (key.ToLower() != "page")
                {
                    rv.Add(key, httpRequestBase.QueryString[key]);
                }
            }


            var urlHelper = new UrlHelper(httpRequestBase.RequestContext);
            //  return urlHelper.Action("BoatsSearch", "Directory", rv);
            return urlHelper.Action(ownerType.SearchAction, ownerType.Controller, rv);


        }
    }
}
