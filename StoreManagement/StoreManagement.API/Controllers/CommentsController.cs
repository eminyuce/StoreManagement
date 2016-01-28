using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Service.IGeneralRepositories;
using WebApi.OutputCache.V2;

namespace StoreManagement.API.Controllers
{
    [CacheOutput(ClientTimeSpan = StoreConstants.CacheClientTimeSpanSeconds, ServerTimeSpan = StoreConstants.CacheServerTimeSpanSeconds)]
    public class CommentsController : BaseApiController<Comment>, ICommentGeneralRepository
    {
        public override IEnumerable<Comment> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Comment Get(int id)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Post(Comment value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Put(int id, Comment value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Comment>> GetCommentsByItemIdAsync(int storeId, int itemId, string itemType, int page, int pageSize)
        {
            return await CommentRepository.GetCommentsByItemIdAsync(storeId, itemId, itemType, page, pageSize);
        }


    }
}