using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.Interfaces
{
    public interface ICommentService : IService
    {
        Task<List<Comment>> GetCommentsByItemIdAsync(int storeId, int itemId, string itemType, int page, int pageSize);
    }
}
