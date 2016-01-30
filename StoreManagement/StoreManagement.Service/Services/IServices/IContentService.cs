using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.RequestModel;

namespace StoreManagement.Service.Services.IServices
{
    public interface IContentService : IBaseService
    {
        ContentDetailViewModel GetContentDetail(string id, string contentType);

        ContentsViewModel GetContentIndexPage(int page, String contentType);
    }
}
