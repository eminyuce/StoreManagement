using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcPaging;
using StoreManagement.Data.Entities;

namespace StoreManagement.Data.RequestModel
{
    public class NavigationViewModel
    {
        public List<Navigation> Navigations { get; set; }
    }
}
