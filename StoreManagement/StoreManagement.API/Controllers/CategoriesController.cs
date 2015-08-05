using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StoreManagement.Data.HelpersModel;
using StoreManagement.Data.Paging;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.API.Controllers
{
    public class CategoriesController : BaseApiController<Category>, ICategoryService
    {
        // GET api/Default1
        public IEnumerable<Category> GetCategories(int storeId)
        {
            return this.CategoryRepository.GetCategoriesByStoreId(storeId);
        }

        // GET api/Default1/5
        public override IEnumerable<Category> GetAll()
        {
            return this.CategoryRepository.GetAll();
        }

        public override Category Get(int id)
        {
            Category category = this.CategoryRepository.GetCategory(id);
            if (category == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return category;
        }

        // PUT api/Default1/5
        public override HttpResponseMessage Put(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != category.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }


            this.CategoryRepository.Edit(category);
            try
            {
                this.CategoryRepository.Save();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Default1
        public override HttpResponseMessage Post(Category category)
        {
            if (ModelState.IsValid)
            {

                this.CategoryRepository.Add(category);
                this.CategoryRepository.Save();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, category);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = category.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Default1/5
        public override HttpResponseMessage Delete(int id)
        {
            Category category = this.CategoryRepository.GetSingle(id);
            if (category == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            this.CategoryRepository.Delete(category);

            try
            {
                this.CategoryRepository.Save();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, category);
        }

        public List<Category> GetCategoriesByStoreId(int storeId)
        {
            return CategoryRepository.GetCategoriesByStoreId(storeId);
        }

        public List<Category> GetCategoriesByStoreIdWithContent(int storeId)
        {
            return CategoryRepository.GetCategoriesByStoreIdWithContent(storeId);
        }


        public List<Category> GetCategoriesByStoreId(int storeId, string type, bool? isActive)
        {
            return CategoryRepository.GetCategoriesByStoreId(storeId, type, isActive);
        }

        public List<Category> GetCategoriesByStoreId(int storeId, string type, string search)
        {
            throw new NotImplementedException();
        }

        public List<Category> GetCategoriesByType(string type)
        {
            throw new NotImplementedException();
        }

        public List<Category> GetCategoriesByStoreIdFromCache(int storeId, string type)
        {
            return CategoryRepository.GetCategoriesByStoreIdFromCache(storeId, type);
        }


        public Category GetCategory(int id)
        {
            return CategoryRepository.GetCategory(id);
        }

        public StorePagedList<Category> GetCategoryWithContents(int categoryId, int page, int pageSize)
        {
            return CategoryRepository.GetCategoryWithContents(categoryId, page, pageSize);
        }

        public async Task<StorePagedList<Category>> GetCategoryWithContentsAsync(int categoryId, int page, int pageSize = 25)
        {
            return await CategoryRepository.GetCategoryWithContentsAsync(categoryId, page, pageSize);
        }

        public async Task<List<Category>> GetCategoriesByStoreIdAsync(int storeId)
        {
            return await CategoryRepository.GetCategoriesByStoreIdAsync(storeId);
        }



        public async Task<List<Category>> GetCategoriesByStoreIdAsync(int storeId, string type, bool? isActive)
        {
            return await CategoryRepository.GetCategoriesByStoreIdAsync(storeId, type, isActive);
        }

        public async Task<Category> GetCategoryAsync(int id)
        {
            return await CategoryRepository.GetCategoryAsync(id);
        }

        public async Task<Category> GetCategoryByContentIdAsync(int storeId, int contentId)
        {
            return await CategoryRepository.GetCategoryByContentIdAsync(storeId, contentId);
        }
    }
}
