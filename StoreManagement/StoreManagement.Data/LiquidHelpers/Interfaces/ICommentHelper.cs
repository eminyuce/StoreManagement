using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;

namespace  StoreManagement.Data.LiquidHelpers.Interfaces
{
    public interface ICommentHelper : IHelper
    {
        StoreLiquidResult GetCommentsPartial(List<Comment> comments, PageDesign pageDesign);
    }
}
