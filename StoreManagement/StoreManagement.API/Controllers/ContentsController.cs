using System.Threading.Tasks;
using StoreManagement.Data.Constants;
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
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.HelpersModel;
using StoreManagement.Data.Paging;
using StoreManagement.Service.Interfaces;
using WebApi.OutputCache.V2;

namespace StoreManagement.API.Controllers
{
    [CacheOutput(ClientTimeSpan = StoreConstants.CacheClientTimeSpanSeconds, ServerTimeSpan = StoreConstants.CacheServerTimeSpanSeconds)]
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

        public List<Content> GetContentByType(string typeName)
        {
            return this.ContentRepository.GetContentByType(typeName);
        }

        public List<Content> GetContentByType(int storeId, string typeName)
        {
            return this.ContentRepository.GetContentByType(storeId, typeName);
        }



 

        public List<Content> GetContentByTypeAndCategoryId(int storeId, string typeName, int categoryId, string search, bool? isActive)
        {
            return this.ContentRepository.GetContentByTypeAndCategoryId(storeId, typeName, categoryId, search, isActive);
        }

 

        public StorePagedList<Content> GetContentsCategoryId(int storeId, int? categoryId, string typeName, bool? isActive, int page, int pageSize)
        {
            var items = this.ContentRepository.GetContentsCategoryId(storeId,
                categoryId,
                typeName,
                isActive,
                page,
                pageSize);

            return items;
        }

        public Content GetContentWithFiles(int id)
        {
            return this.ContentRepository.GetContentWithFiles(id);
        }



        public async Task<StorePagedList<Content>> GetContentsCategoryIdAsync(int storeId, int? categoryId, string typeName, bool? isActive, int page, int pageSize,
                                               string search)
        {
            var items = await this.ContentRepository.GetContentsCategoryIdAsync(storeId,
                         categoryId,
                         typeName,
                         isActive,
                         page,
                         pageSize, search);

            return items;
        }

        public async Task<Content> GetContentByIdAsync(int id)
        {
            return await this.ContentRepository.GetContentByIdAsync(id);
        }



        public async Task<List<Content>> GetContentByTypeAndCategoryIdAsync(int storeId, string typeName, int categoryId, int take, int? excludedContentId)
        {
            return await this.ContentRepository.GetContentByTypeAndCategoryIdAsync(storeId, typeName, categoryId, take, excludedContentId);
        }

        public async Task<List<Content>> GetMainPageContentsAsync(int storeId, int? categoryId, string type, int? take)
        {
            return await this.ContentRepository.GetMainPageContentsAsync(storeId, categoryId, type, take);
        }

        public async Task<List<Content>> GetContentByTypeAsync(int storeId, int? take, bool? isActive, string typeName)
        {
            return await this.ContentRepository.GetContentByTypeAsync(storeId, take, isActive, typeName);
        }

        public async Task<List<Content>> GetContentsByContentKeywordAsync(int storeId, int? catId, string type, int page, int pageSize, bool? isActive,
                                                     string contentType)
        {
            return
                await ContentRepository.GetContentsByContentKeywordAsync(storeId, catId, type, page, pageSize, isActive, contentType);
        }
    }
}