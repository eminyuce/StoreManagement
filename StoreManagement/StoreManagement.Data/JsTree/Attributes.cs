using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.JsTree
{
    public class Attributes
    {
        string _id;

        public string id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        string _rel;

        public string rel
        {
            get
            {
                return _rel;
            }
            set
            {
                _rel = value;
            }
        }
        string _mdata;

        public string mdata
        {
            get
            {
                return _mdata;
            }
            set
            {
                _mdata = value;
            }
        }
    }

}
