using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Controllers
{
    public class CategoriesController : BaseController
    {

        [Inject]
        public ICategoryRepository CategoryRepository { set; get; }

        [Inject]
        public IContentRepository ContentRepository { set; get; }

        //
        // GET: /Categories/
        public CategoriesController(IStoreContext dbContext, 
            ISettingRepository settingRepository, 
            IStoreRepository storeRepository) : base(dbContext, settingRepository, storeRepository)
        {

        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Category(String id, int page=1)
        {
            var returnModel = new CategoryViewModel();
            int categoryId = id.Split("-".ToCharArray()).Last().ToInt();

            returnModel.Store = store;
            returnModel.Category = CategoryRepository.GetSingle(categoryId);
            returnModel.Contents =  ContentRepository.GetContentsCategoryId(store.Id, categoryId, true);


            return View(returnModel);

        }
	}
}