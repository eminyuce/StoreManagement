using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.JsTree
{
    public class Data
    {
        string _title;

        public string title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }

        string _icon;

        public string icon
        {
            get
            {
                return _icon;
            }
            set
            {
                _icon = value;
            }
        }
    }

}
