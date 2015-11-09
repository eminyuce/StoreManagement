using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Data.HelpersModel
{
    public class Filter: BaseDrop
    {
        public string FilterLink { get; set; }

        public String FieldName { get; set; }
        private string _valueFirst = "";
        public String ValueFirst { get { return _valueFirst; } set { _valueFirst = value; } }

        public String ValueLast { get; set; }

        public int Cnt { get; set; }
        public int Ord { get; set; }

        private string _text = "";
        public string Text
        {
            get
            {
                if (string.IsNullOrEmpty(_text))
                {
                    if (!String.IsNullOrEmpty(ValueLast))
                    {
                        if (ValueFirst == ValueLast)
                        {
                            return ValueFirst;
                        }
                        else
                        {

                            return ValueFirst + " - " + ValueLast;
                        }
                    }
                    else
                    {
                        return ValueFirst;
                    }


                }
                else
                {
                    return _text;
                }
            }
            set { _text = value; }
        }


        public Filter()
        {

        }
        public string Url
        {
            get
            {
                //  Uri.EscapeDataString()
                //if(!string.IsNullOrEmpty(ValueLast) && ValueFirst!=ValueLast)
                //{
                string url = FieldName.UrlEncode() + "-";
                if (ValueFirst == ValueLast)
                {
                    url += ValueFirst.UrlEncode();
                }
                else
                {
                    url += ValueFirst.UrlEncode() + (!string.IsNullOrEmpty(ValueLast) ? "-" + ValueLast.UrlEncode() : "");
                }


                return url.Trim();

                // }


            }
        }

        public Filter(string fieldName, string valueFirst, string valueLast)
        {
            // TODO: Complete member initialization
            this.FieldName = fieldName;
            this.ValueFirst = valueFirst;
            this.ValueLast = valueLast;
        }

        private ItemType _ownerType ;
        public ItemType OwnerType { get { return _ownerType; } set { _ownerType = value; } }


        public string Link(HttpRequestBase httpRequestBase)
        {

            string sFilters = (string)httpRequestBase.RequestContext.RouteData.Values["filters"];
            var filters = FilterHelper.ParseFiltersFromString(sFilters);

            string urlFilters;

            if (filters != null && filters.Count() > 0)
            {
                if (!filters.Any(i => i.FieldName.ToLower() == FieldName.ToLower()))
                {
                    filters.Add(this);
                }

                urlFilters = string.Join("/",
                                         filters.OrderBy(i => i.FieldName).Select(
                                             i => (i.FieldName.ToLower() == FieldName.ToLower()) ? Url : i.Url));
            }
            else
            {
                urlFilters = Url;
            }



            var rv = new RouteValueDictionary();
            rv.Add("filters", urlFilters);

            foreach (var key in httpRequestBase.QueryString.AllKeys)
            {
                if (key != null)
                {
                    if (key.ToLower() != "page")
                    {
                        if (!rv.ContainsKey(key))
                        {
                            rv.Add(key, httpRequestBase.QueryString[key]);
                        }
                    }
                }
            }



            var urlHelper = new UrlHelper(httpRequestBase.RequestContext);
            return urlHelper.Action(OwnerType.SearchAction, OwnerType.Controller, rv).ToLower();
        }
         
    }
}
