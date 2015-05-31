﻿using StoreManagement.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StoreManagement.Data.JsTree;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.API.Controllers
{
    public class CategoriesController : BaseApiController, ICategoryService
    {
        // GET api/Default1
        public IEnumerable<Category> GetCategories(int storeId)
        {
            return this.CategoryRepository.GetCategoriesByStoreId(storeId);
        }

        // GET api/Default1/5
        public Category GetCategory(int id)
        {
            Category category = this.CategoryRepository.GetSingle(id);
            if (category == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return category;
        }

        // PUT api/Default1/5
        public HttpResponseMessage PutCategory(int id, Category category)
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
        public HttpResponseMessage PostCategory(Category category)
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
        public HttpResponseMessage DeleteCategory(int id)
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

        public List<Category> GetCategoriesByStoreId(int storeId, string type)
        {
            return CategoryRepository.GetCategoriesByStoreId(storeId, type);
        }

        public List<Category> GetCategoriesByStoreIdFromCache(int storeId, string type)
        {
            return CategoryRepository.GetCategoriesByStoreIdFromCache(storeId, type);
        }

        public List<JsTreeNode> CreateCategoriesTree(int storeId, string type)
        {
            return CategoryRepository.CreateCategoriesTree(storeId, type);
        }

        public Category GetSingle(int id)
        {
            return CategoryRepository.GetSingle(id);
        }
    }
}
