﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Service.IGeneralRepositories;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface ILabelLineRepository : IBaseRepository<LabelLine, int>, ILabelLineGeneralRepository, IDisposable 
    {

        void DeleteLabelLinesByItem(int itemId, String itemType);
        void SaveLabelLines(int[] labelId, int itemId, String itemType);

    }
}
