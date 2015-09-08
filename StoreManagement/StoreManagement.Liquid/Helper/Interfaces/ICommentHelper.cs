using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Liquid.Helper.Interfaces
{
    public interface ICommentHelper : IHelper
    {
        StoreLiquidResult GetCommentsPartial(Task<List<Comment>> commentsTask, Task<PageDesign> pageDesignTask);
    }
}
