using StoreManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace StoreManagement.API.Controllers
{
    public class ContentsController : BaseApiController
    {
        // GET api/Contents
        public IEnumerable<Content> GetContents(int storeId, String typeName)
        {
            return this.ContentRepository.GetContentByType(storeId, typeName);
        }

        // GET api/Contents/5
        public Content GetContent(int id)
        {
            Content content = this.ContentRepository.GetSingle(id);
            if (content == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return content;
        }

        // PUT api/Contents/5
        public HttpResponseMessage PutContent(int id, Content content)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != content.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            this.ContentRepository.Edit(content);

            try
            {
                this.ContentRepository.Save();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Contents
        public HttpResponseMessage PostContent(Content content)
        {
            if (ModelState.IsValid)
            {
                this.ContentRepository.Add(content);
                        this.ContentRepository.Save();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, content);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = content.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Contents/5
        public HttpResponseMessage DeleteContent(int id)
        {
            Content content = this.ContentRepository.GetSingle(id);
            if (content == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            this.ContentRepository.Delete(content);

            try
            {
                        this.ContentRepository.Save();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, content);
        }

       
    }
}