﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.Interfaces
{
    public interface IRetailerService : IService
    {
        Task<List<Retailer>> GetRetailersAsync(int storeId, int ? take, bool isActive);
    }
}
