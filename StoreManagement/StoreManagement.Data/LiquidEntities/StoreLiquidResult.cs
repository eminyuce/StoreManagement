using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Constants;

namespace StoreManagement.Data.LiquidEntities
{
    public class StoreLiquidResult
    {

        public Dictionary<String, String> LiquidRenderedResult { get; set; }
 
        public String PageOutputText
        {
            get
            {
                return LiquidRenderedResult[StoreConstants.PageOutput];
            }
        }
    }
}
