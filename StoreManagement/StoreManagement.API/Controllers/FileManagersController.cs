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
    public class FileManagersController : BaseApiController<FileManager>, IFileManagerService
    {

        // GET api/FileManagers
        public IEnumerable<FileManager> GetFileManagers(int storeId)
        {
            return this.FileManagerRepository.GetFilesByStoreId(storeId);
        }

        // GET api/FileManagers/5
        public override IEnumerable<FileManager> GetAll()
        {
            return this.FileManagerRepository.GetAll();
        }

        public override FileManager Get(int id)
        {
            FileManager filemanager = this.FileManagerRepository.GetSingle(id);
            if (filemanager == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return filemanager;
        }

        // PUT api/FileManagers/5
        public override HttpResponseMessage Put(int id, FileManager filemanager)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != filemanager.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            this.FileManagerRepository.Edit(filemanager);

            try
            {
                this.FileManagerRepository.Save();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/FileManagers
        public override HttpResponseMessage Post(FileManager filemanager)
        {
            if (ModelState.IsValid)
            {
                this.FileManagerRepository.Add(filemanager);
                this.FileManagerRepository.Save();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, filemanager);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = filemanager.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/FileManagers/5
        public override HttpResponseMessage Delete(int id)
        {
            FileManager filemanager = this.FileManagerRepository.GetSingle(id);
            if (filemanager == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            this.FileManagerRepository.Delete(filemanager);

            try
            {
                this.FileManagerRepository.Save();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, filemanager);
        }

        public List<FileManager> GetFilesByStoreIdFromCache(int storeId)
        {
            return FileManagerRepository.GetFilesByStoreIdFromCache(storeId);
        }

        public List<FileManager> GetFilesByStoreId(int storeId)
        {
            return FileManagerRepository.GetFilesByStoreId(storeId);
        }

        public FileManager GetFilesByGoogleImageId(string googleImageId)
        {
            return FileManagerRepository.GetFilesByGoogleImageId(googleImageId);
        }

        public List<FileManager> GetStoreCarousels(int storeId)
        {
            throw new NotImplementedException();
        }
    }
}