using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ninject;
using NLog;
using StoreManagement.Data;
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
        public IProductRepository ProductRepository { set; get; }

        [Inject]
        public IProductFileRepository ProductFileRepository { set; get; }

        [Inject]
        public IProductCategoryRepository ProductCategoryRepository { set; get; }

        [Inject]
        public ILabelLineRepository LabelLineRepository { set; get; }

        [Inject]
        public ILabelRepository LabelRepository { set; get; }

        [Inject]
        public IBrandRepository BrandRepository { set; get; }

        [Inject]
        public ILogRepository LogRepository { set; get; }

        [Inject]
        public IStoreLanguageRepository StoreLanguageRepository { set; get; }


        [Inject]
        public IContactRepository ContactRepository { set; get; }

        private bool _isCacheEnable = true;
        public bool IsCacheEnable
        {
            get { return _isCacheEnable; }
            set { _isCacheEnable = value; }
        }
        private int _cacheMinute = 30;
        public int CacheMinute
        {
            get { return _cacheMinute; }
            set { _cacheMinute = value; }
        }


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
