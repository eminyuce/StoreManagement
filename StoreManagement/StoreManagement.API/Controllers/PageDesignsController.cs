using StoreManagement.API.Controllers;
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
    public class PageDesignsController : BaseApiController<PageDesign>, IPageDesignService
    {

        // GET api/PageDesigns/5
        public override IEnumerable<PageDesign> GetAll()
        {
            return this.PageDesignRepository.GetAll();
        }

        public override PageDesign Get(int id)
        {
            PageDesign pagedesign = this.PageDesignRepository.GetSingle(id);
            if (pagedesign == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return pagedesign;
        }

        // PUT api/PageDesigns/5
        public override HttpResponseMessage Put(int id, PageDesign pagedesign)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != pagedesign.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            this.PageDesignRepository.Edit(pagedesign);

            try
            {
                this.PageDesignRepository.Save();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/PageDesigns
        public override HttpResponseMessage Post(PageDesign pagedesign)
        {
            if (ModelState.IsValid)
            {
                this.PageDesignRepository.Add(pagedesign);
                this.PageDesignRepository.Save();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, pagedesign);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = pagedesign.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/PageDesigns/5
        public override HttpResponseMessage Delete(int id)
        {
            PageDesign pagedesign = this.PageDesignRepository.GetSingle(id);
            if (pagedesign == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            this.PageDesignRepository.Delete(pagedesign);

            try
            {
                this.PageDesignRepository.Save();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, pagedesign);
        }
 

        public PageDesign GetPageDesignByName(int storeId, string name)
        {
            return PageDesignRepository.GetPageDesignByName(storeId, name);
        }
    }
}