using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Services
{
    public class ContentService : BaseService, IContentService
    {
        public ContentService(string webServiceAddress) : base(webServiceAddress)
        {

        }

        public Content GetContentsContentId(int contentId)
        {
            throw new NotImplementedException();
        }

        public List<Content> GetContentByType(int storeId, string typeName)
        {
            throw new NotImplementedException();
        }

        public Content GetContentByUrl(int storeId, string url)
        {
            throw new NotImplementedException();
        }

        public List<Content> GetContentByTypeAndCategoryId(int storeId, string typeName, int categoryId)
        {
            throw new NotImplementedException();
        }

        public List<Content> GetContentByTypeAndCategoryIdFromCache(int storeId, string typeName, int categoryId)
        {
            throw new NotImplementedException();
        }

        public List<Content> GetContentsCategoryId(int storeId, int categoryId, String typeName, bool? isActive)
        {
            throw new NotImplementedException();
        }

        public Content GetContentWithFiles(int id)
        {
            throw new NotImplementedException();
        }
    }
}
