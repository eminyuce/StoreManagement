using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Helpers.GeneralHelper
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
                            if (GeneralHelper.IsNumeric(filter.ValueFirst))
                            { filter.ValueLast = filter.ValueFirst; }


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
            //return urlHelper.Action("Index", "Companies", rv);
        }




        public static List<Filter> GetNavigationFilters(ViewContext viewContext)
        {
            string filters = (string)viewContext.RouteData.Values["filters"];
            var fltrs = FilterHelper.ParseFiltersFromString(filters);

            return fltrs.OrderBy(i => i.FieldName).ToList();


        }
    }
}
