using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ninject;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.API.Controllers
{
    public abstract class BaseApiController : ApiController
    {

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


    }
}
