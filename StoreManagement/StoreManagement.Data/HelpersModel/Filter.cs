using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.GeneralHelper;

namespace StoreManagement.Data.HelpersModel
{
    public class Filter
    {
        public String FieldName { get; set; }
        public String ValueFirst { get; set; }
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


       


    }
}
