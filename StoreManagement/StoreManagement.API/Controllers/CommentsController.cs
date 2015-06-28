using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using StoreManagement.Data.Entities;

namespace StoreManagement.API.Controllers
{
    public class CommentsController:BaseApiController<Comment>
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
    }
}