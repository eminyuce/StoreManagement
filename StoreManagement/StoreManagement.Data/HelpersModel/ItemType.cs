using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.HelpersModel
{
    public class ItemType
    {
        public string Name { get; set; }
        public string SearchCommand { get; set; }
        public Type Type { get; set; }

        public string Controller { get; set; }
        public string SearchAction { get; set; }
        public string DetailsAction { get; set; }

        public string SearchTemplete { get; set; }

        public bool IsInternal { get; set; }


        public string MenuName { get; set; }

        private string _dropDownName = "";
        public string DropDownName
        {
            get { return string.IsNullOrEmpty(_dropDownName) ? MenuName : _dropDownName; }
            set { _dropDownName = value; }
        }

        public bool IsMenuItem { get; set; }

        //public string Url
        //{
        //    get { return DropDownName.UrlEncode(); }
        //}

        public int ItemTypeID { get; set; }

    }
}
