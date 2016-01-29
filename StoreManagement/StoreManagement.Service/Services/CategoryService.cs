using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using StoreManagement.Service.Services.IServices;

namespace StoreManagement.Service.Services
{
    public class CategoryService : BaseService ,  ICategoryService
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    }
}
