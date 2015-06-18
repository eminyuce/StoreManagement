using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Admin.Constants;

namespace StoreManagement.Admin.Controllers
{
    public class BlogLabelsController : LabelsController
    {
        public BlogLabelsController()
            : base(StoreConstants.BlogsType)
        {

        }

        
    }
}