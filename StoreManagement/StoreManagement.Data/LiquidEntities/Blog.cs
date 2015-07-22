using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;
using StoreManagement.Data.Entities;

namespace StoreManagement.Data.LiquidEntities
{
    [LiquidType]
    public class Blog  
    {
        public Content Content { get; set; }
        public Blog(Content content)
        {
            this.Content = content;
        }

    }
}
