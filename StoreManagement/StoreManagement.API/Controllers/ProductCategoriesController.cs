using System.Threading.Tasks;
using StoreManagement.Data.Constants;
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
using StoreManagement.Service.IGeneralRepositories;
using WebApi.OutputCache.V2;

namespace StoreManagement.API.Controllers
{
    [CacheOutput(ClientTimeSpan = StoreConstants.CacheClientTimeSpanSeconds, ServerTimeSpan = StoreConstants.CacheServerTimeSpanSeconds)]
    public class ProductCategoriesController : BaseApiController<ProductCategory>, IProductCategoryGeneralRepository
    {
        // GET api/Default1
        public IEnumerable<ProductCategory> GetProductCategories(int storeId)
        {
            return this.ProductCategoryRepository.GetProductCategoriesByStoreId(storeId);
        }

        // GET api/Default1/5
        public override IEnumerable<ProductCategory> GetAll()
        {
            return this.ProductCategoryRepository.GetAll();
        }

        public override ProductCategory Get(int id)
        {
            ProductCategory category = this.ProductCategoryRepository.GetProductCategory(id);
            if (category == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return category;
        }

        // PUT api/Default1/5
        public override HttpResponseMessage Put(int id, ProductCategory category)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != category.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }


            this.ProductCategoryRepository.Edit(category);
            try
            {
                this.ProductCategoryRepository.Save();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Default1
        public override HttpResponseMessage Post(ProductCategory category)
        {
            if (ModelState.IsValid)
            {

                this.ProductCategoryRepository.Add(category);
                this.ProductCategoryRepository.Save();

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

        public List<ProductCategory> GetProductCategoriesByStoreId(int storeId)
        {
            return ProductCategoryRepository.GetProductCategoriesByStoreId(storeId);
        }

        public List<ProductCategory> GetProductCategoriesByStoreIdWithContent(int storeId)
        {
            return ProductCategoryRepository.GetProductCategoriesByStoreIdWithContent(storeId);
        }

        public List<ProductCategory> GetProductCategoriesByStoreId(int storeId, string type)
        {
            return ProductCategoryRepository.GetProductCategoriesByStoreId(storeId, type);
        }

        public List<ProductCategory> GetProductCategoriesByStoreId(int storeId, string type, string search)
        {
            throw new NotImplementedException();
        }

        public List<ProductCategory> GetProductCategoriesByStoreIdFromCache(int storeId, string type)
        {
            return ProductCategoryRepository.GetProductCategoriesByStoreIdFromCache(storeId, type);
        }

        public ProductCategory GetProductCategory(int id)
        {
            return ProductCategoryRepository.GetProductCategory(id);
        }

        public StorePagedList<ProductCategory> GetProductCategoryWithContents(int categoryId, int page, int pageSize)
        {
            return ProductCategoryRepository.GetProductCategoryWithContents(categoryId, page, pageSize);
        }

        public Task<List<ProductCategory>> GetProductCategoriesByStoreIdAsync(int storeId, string type, bool? isActive)
        {
            return ProductCategoryRepository.GetProductCategoriesByStoreIdAsync(storeId, type, isActive);
        }

        public Task<StorePagedList<ProductCategory>> GetProductCategoriesByStoreIdAsync(int storeId, string type, bool? isActive, int page, int pageSize = 25)
        {
            return ProductCategoryRepository.GetProductCategoriesByStoreIdAsync(storeId, type, isActive, page, pageSize);
        }

        public Task<ProductCategory> GetProductCategoryAsync(int storeId, int productId)
        {
            return ProductCategoryRepository.GetProductCategoryAsync(storeId, productId);
        }

        public Task<ProductCategory> GetProductCategoryAsync(int categoryId)
        {
            return ProductCategoryRepository.GetProductCategoryAsync(categoryId);
        }

        public Task<List<ProductCategory>> GetCategoriesByBrandIdAsync(int storeId, int brandId)
        {
            return ProductCategoryRepository.GetCategoriesByBrandIdAsync(storeId, brandId);
        }

        public Task<List<ProductCategory>> GetCategoriesByRetailerIdAsync(int storeId, int retailerId)
        {
            return ProductCategoryRepository.GetCategoriesByRetailerIdAsync(storeId, retailerId);
        }
    }
}
