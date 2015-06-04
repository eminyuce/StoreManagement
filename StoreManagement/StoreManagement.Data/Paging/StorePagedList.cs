using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Data.Paging
{
    /// <summary>
    /// A class that handles pagination of an IQueryable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    // [Serializable]
    public class StorePagedList<T> where T : new()
    {

        public StorePagedList()
        {
        
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="list">The full list of items you would like to paginate</param>
        /// <param name="page">(optional) The current page number</param>
        /// <param name="pageSize">(optional) The size of the page</param>
        public StorePagedList(List<T> list, int page = 0, int pageSize = 0, int totalItemCount = 0)
        {
            this.page = page;
            this.pageSize = pageSize;
            this.totalItemCount = totalItemCount;
            this.items = list;
        }

        private List<T> _list { set; get; }

        /// <summary>
        /// The paginated result
        /// </summary>
        public List<T> items
        {
            get
            {
                if (_list == null) return null;
                return _list;
                //   return _list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
            set { _list = value; }
        }

        private int _page { set; get; }
        /// <summary>
        ///  The current page.
        /// </summary>
        public int page
        {
            get { return _page; }
            set { _page = value; }
        }

        private int _pageSize { set; get; }
        /// <summary>
        /// The size of the page.
        /// </summary>
        public int pageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        private int _totalItemCount { set; get; }
        /// <summary>
        /// The total number of items in the original list of items.
        /// </summary>
        public int totalItemCount
        {
            get { return _totalItemCount; }
            set { _totalItemCount = value; }
        }
    }
}
