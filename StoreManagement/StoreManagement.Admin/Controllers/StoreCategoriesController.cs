using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;

namespace StoreManagement.Admin.Controllers
{
    public class StoreCategoriesController : CategoriesController
    {
        public StoreCategoriesController() : base(StoreConstants.StoreType)
        {
           


        }

        
        public override ActionResult Index(int storeId = 0, string search = "")
        {
            List<Category> resultList = new List<Category>();
           
            
                resultList = CategoryRepository.GetCategoriesByType(StoreConstants.StoreType);

            if (!String.IsNullOrEmpty(search))
            {
                resultList = resultList.Where(r => r.Name.ToLower().Contains(search.ToLower())).ToList();
            }

            return View(resultList);
        }
    }
}