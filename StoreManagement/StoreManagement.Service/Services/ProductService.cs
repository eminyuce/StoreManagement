using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using StoreManagement.Service.IGeneralRepositories;
using StoreManagement.Service.Services.IServices;

namespace StoreManagement.Service.Services
{
    public class ProductService :BaseService, IProductService 
    {
        [Inject]
        public IProductGeneralRepository ProductRepository { set; get; }


    }
}
