using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ninject;
using NLog;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.API.Controllers
{
    public abstract class BaseApiController<T> : ApiController where T : class
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        [Inject]
        public IFileManagerRepository FileManagerRepository { get; set; }

        [Inject]
        public IContentFileRepository ContentFileRepository { set; get; }

        [Inject]
        public IContentRepository ContentRepository { set; get; }

        [Inject]
        public ICategoryRepository CategoryRepository { set; get; }

        [Inject]
        public IStoreRepository StoreRepository { set; get; }

        [Inject]
        public INavigationRepository NavigationRepository { set; get; }

        [Inject]
        public IPageDesignRepository PageDesignRepository { set; get; }

        [Inject]
        public IStoreUserRepository StoreUserRepository { set; get; }
        
        [Inject]
        public ISettingRepository SettingRepository { set; get; }

        [Inject]
        public IStoreCarouselRepository StoreCarouselRepository { set; get; }



        // GET api/<controller>
        public abstract IEnumerable<T> GetAll();
        // GET api/<controller>/5
        public abstract T Get(int id);
        // POST api/<controller>
        public abstract HttpResponseMessage Post([FromBody] T value);
        // PUT api/<controller>
        public abstract HttpResponseMessage Put(int id, [FromBody] T value);
        // DELETE api/<controller>/5
        public abstract HttpResponseMessage Delete(int id);


    }
}
