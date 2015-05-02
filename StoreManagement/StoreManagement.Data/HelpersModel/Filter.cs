using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StoreManagement.Data.HelpersModel
{
    public class Filter
    {
        public String FieldName { get; set; }
        public String ValueFirst { get; set; }
        public String ValueLast { get; set; }
        public int Cnt { get; set; }

        private string _text = "";
     
        public string Text
        {
            get
            {
                if (string.IsNullOrEmpty(_text))
                {

                    if (ValueFirst == ValueLast)
                    {
                        return ValueFirst;
                    }
                    else
                    {
                        if (!GeneralHelper.IsNumeric(ValueLast))
                        {

                            return (!string.IsNullOrEmpty(ValueLast) ? ValueLast + ", " : "") + ValueFirst;
                        }
                        else
                        {
                            return ValueFirst + " - " + ValueLast;
                        }

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

        public Filter(string fieldName, string valueFirst, string valueLast)
        {
            // TODO: Complete member initialization
            this.FieldName = fieldName;
            this.ValueFirst = valueFirst;
            this.ValueLast = valueLast;
        }

        private ItemType _ownerType ;
        public ItemType OwnerType { get { return _ownerType; } set { _ownerType = value; } }


        public double DoubleValueFirst
        {
            get { return ValueFirst.ToDouble(); }
            set { ValueFirst = value.ToString(); }
        }
        public double DoubleValueLast
        {
            get { return ValueLast.ToDouble(); }
            set { ValueLast = value.ToString(); }
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

        public string LinkExclude(HttpRequestBase httpRequestBase, ViewContext viewContext, ItemType ownerType)
        {
            //RequestContext
            string sFilters = (string)viewContext.RouteData.Values["filters"];
            var filters = FilterHelper.ParseFiltersFromString(sFilters);


            var rv = new RouteValueDictionary();


            if (filters != null)
            {
                int index = filters.FindIndex(i => i.FieldName.ToLower() == FieldName.ToLower());
                if (index >= 0)
                {
                    filters.RemoveAt(index);
                }

                string urlFilters = string.Join("/",
                                        filters.OrderBy(i => i.FieldName).Select(
                                            i => (i.FieldName.ToLower() == FieldName.ToLower()) ? Url : i.Url));

                rv.Add("filters", urlFilters);
            }




            foreach (var key in httpRequestBase.QueryString.AllKeys)
            {
                if (key.ToLower() != "page")
                {
                    if (!rv.ContainsKey(key))
                    {
                        rv.Add(key, httpRequestBase.QueryString[key]);
                    }
                }
            }


            var urlHelper = new UrlHelper(httpRequestBase.RequestContext);
            //  return urlHelper.Action("BoatsSearch", "Directory", rv);
            return urlHelper.Action(ownerType.SearchAction, ownerType.Controller, rv);


        }

        public string Link(HttpRequestBase httpRequestBase, ViewContext viewContext)
        {

            string sFilters = (string)viewContext.RouteData.Values["filters"];
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
                if (key.ToLower() != "page")
                {
                    if (!rv.ContainsKey(key))
                    {
                        rv.Add(key, httpRequestBase.QueryString[key]);
                    }
                }
            }



            var urlHelper = new UrlHelper(httpRequestBase.RequestContext);
            return urlHelper.Action(OwnerType.SearchAction, OwnerType.Controller, rv);
        }



    }
}
