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
using StoreManagement.Service.Interfaces;

namespace StoreManagement.API.Controllers
{
    public class ContentsController : BaseApiController<Content>, IContentService
    {
        // GET api/Contents
        public IEnumerable<Content> GetContents(int storeId, String typeName)
        {
            return this.ContentRepository.GetContentByType(storeId, typeName);
        }

        // GET api/Contents/5
        public override IEnumerable<Content> GetAll()
        {
            return this.ContentRepository.GetAll();
        }

        public override Content Get(int id)
        {
            Content content = this.ContentRepository.GetSingle(id);
            if (content == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return content;
        }

        // PUT api/Contents/5
        public override HttpResponseMessage Put(int id, Content content)
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
        public override HttpResponseMessage Post(Content content)
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
        public override HttpResponseMessage Delete(int id)
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


        public Content GetContentsContentId(int contentId)
        {
            return this.ContentRepository.GetContentsContentId(contentId);
        }

        public List<Content> GetContentByType(int storeId, string typeName)
        {
            return this.ContentRepository.GetContentByType(storeId, typeName);
        }

        public Content GetContentByUrl(int storeId, string url)
        {
            return this.ContentRepository.GetContentByUrl(storeId, url);
        }

        public List<Content> GetContentByTypeAndCategoryId(int storeId, string typeName, int categoryId)
        {
            return this.ContentRepository.GetContentByTypeAndCategoryId(storeId, typeName, categoryId);
        }

        public List<Content> GetContentByTypeAndCategoryIdFromCache(int storeId, string typeName, int categoryId)
        {
            return this.ContentRepository.GetContentByTypeAndCategoryIdFromCache(storeId, typeName, categoryId);
        }

        public List<Content> GetContentsCategoryId(int storeId, int categoryId, string typeName, bool? isActive)
        {
            return this.ContentRepository.GetContentsCategoryId(storeId, categoryId, typeName, isActive);
        }

        public Content GetContentWithFiles(int id)
        {
            return this.ContentRepository.GetContentWithFiles(id);
        }
    }
}