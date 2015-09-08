using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.API.Controllers
{
    public class CommentsController : BaseApiController<Comment>, ICommentService
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