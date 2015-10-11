using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.API.Controllers
{
    public class MessagesController : BaseApiController<Message>, IMessageService
    {
        public override IEnumerable<Message> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Message Get(int id)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Post(Message value)
        {
            if (ModelState.IsValid)
            {

                MessageRepository.Add(value);
                MessageRepository.Save();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, value);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = value.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        public override HttpResponseMessage Put(int id, Message value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void SaveContactFormMessage(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
