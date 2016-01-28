using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.Services.IServices
{
    public interface IBaseService
    {
        Store MyStore { set; get; }
        int StoreId { get; set; }
        String StoreName { get; set; }
    }
}
