using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.Paging;

namespace StoreManagement.Data.LiquidEntities
{
    public class PaginatorLiquid : Drop
    {

        public int TotalPages { get { return TotalRecords / PageSize; } }
        public bool PreviousPage { get { return Page != 1; } }
        public String PreviousPagePath { get { return PaginatePath.Replace(":num", (Page - 1).ToStr()); } }
        public int Page { get; set; }
        public String PaginatePath { get; set; }
        public bool NextPage { get { return Page != TotalPages; } }
        public String NextPagePath { get { return PaginatePath.Replace(":num", (Page + 1).ToStr()); } }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }
}
