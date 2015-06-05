﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.HelpersModel;
using StoreManagement.Data.Paging;

namespace StoreManagement.Service.Interfaces
{
    public interface IContentService : IService
    {

        Content GetContentsContentId(int contentId);
        List<Content> GetContentByType(String typeName);
        List<Content> GetContentByType(int storeId, String typeName);
        List<Content> GetContentByTypeAndCategoryId(int storeId, String typeName, int categoryId);
        List<Content> GetContentByTypeAndCategoryIdFromCache(int storeId, String typeName, int categoryId);
        StorePagedList<Content> GetContentsCategoryId(int storeId, int categoryId, String typeName, bool? isActive, int page, int pageSize);
        Content GetContentWithFiles(int id);
 
    }
}
