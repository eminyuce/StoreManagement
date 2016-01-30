using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.RequestModel;

namespace StoreManagement.Service.Services.IServices
{
    public interface IProductCategoryService : IBaseService
    { 
        ProductCategoryViewModel GetProductCategory(string id, int page);
    }
}
