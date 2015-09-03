using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.HelpersModel
{
    public class OrderingItem
    {
        public int Id { get; set; }
        public int Ordering { get; set; }
        public bool State { get; set; }

        public override string ToString()
        {
            return "Id:" + Id + " Ordering:" + Ordering + " State:" + State;
        }

    }
}
