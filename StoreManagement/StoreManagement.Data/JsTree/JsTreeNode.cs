using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.JsTree
{

    public class JsTreeNode
    {
        public JsTreeNode()
        {
        }
        Attributes _attributes;
        public Attributes attributes
        {
            get
            {
                return _attributes;
            }
            set
            {
                _attributes = value;
            }
        }
        Data _data;
        public Data data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }
        string _state;
        public string state
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }
        List<JsTreeNode> _children;
        public List<JsTreeNode> children
        {
            get
            {
                return _children;
            }
            set
            {
                _children = value;
            }
        }

    }
}
