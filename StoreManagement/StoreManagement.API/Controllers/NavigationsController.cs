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
    public class NavigationsController : BaseApiController
    {

        // GET api/Navigations
        public IEnumerable<Navigation> GetNavigations(int storeId)
        {
            return this.NavigationRepository.GetStoreNavigations(storeId);
        }

        // GET api/Navigations/5
        public Navigation GetNavigation(int id)
        {
            Navigation navigation = this.NavigationRepository.GetSingle(id);
            if (navigation == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return navigation;
        }

        // PUT api/Navigations/5
        public HttpResponseMessage PutNavigation(int id, Navigation navigation)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != navigation.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            this.NavigationRepository.Edit(navigation);

            try
            {
                this.NavigationRepository.Save();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Navigations
        public HttpResponseMessage PostNavigation(Navigation navigation)
        {
            if (ModelState.IsValid)
            {
                this.NavigationRepository.Add(navigation);
                this.NavigationRepository.Save();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, navigation);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = navigation.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Navigations/5
        public HttpResponseMessage DeleteNavigation(int id)
        {
            Navigation navigation = this.NavigationRepository.GetSingle(id);
            if (navigation == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            this.NavigationRepository.Delete(navigation);

            try
            {
                this.NavigationRepository.Save();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, navigation);
        }

    }
}