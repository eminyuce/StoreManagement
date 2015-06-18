using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using StoreManagement.Admin.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Admin.Controllers
{
    [Authorize]
    public class BlogsController : ContentsController
    {
        public BlogsController()
            : base(StoreConstants.BlogsType)
        {
            
        }
    }
}