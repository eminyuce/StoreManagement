using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Data.RequestModel
{
    public class VmDashboard : BaseDrop
    {
        public int RateVisitors { get; set; }
        public int RatePageViews { get; set; }
        public int RateUsers { get; set; }

        public int RateOrders { get; set; }

        public List<Tuple<int, string, string, string>> Users;

        public List<Tuple<int, string, string, string>> Orders;

        public List<Tuple<int, string, string, string>> Clients;

        public List<Tuple<int, DateTime, Double>> Invoices;

    }
}
