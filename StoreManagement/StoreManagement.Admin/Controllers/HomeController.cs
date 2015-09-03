using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc;
using System.Web.Security;
using MvcPaging;
using StoreManagement.Data;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;
using StoreManagement.Data.GeneralHelper;
using WebGrease.Css.Ast.Selectors;


namespace StoreManagement.Admin.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {

        static readonly TypedObjectCache<Store> UserStoreCache = new TypedObjectCache<Store>("UserStoreCache");

        static readonly TypedObjectCache<List<BaseEntity>> StoreSearchCache = new TypedObjectCache<List<BaseEntity>>("StoreSearchCache");


        public ActionResult Index()
        {
            return RedirectToAction("Index", "Dashboard");
        }


        public ActionResult NoAccessPage(int id)
        {
            int storeId = id;
            Logger.Info("NoAccessPage. StoreId:" + storeId);
            return View();
        }

        //<li>
        //                           <a href="@url">Go to frontend <i class="glyphicon glyphicon-share-alt"></i></a>
        //                       </li>



        public ActionResult StoreName()
        {
            if (IsSuperAdmin)
            {
                return PartialView("StoreName", "Store Management Admin Panel");
            }
            else
            {
                if (LoginStore == null)
                {
                    return PartialView("StoreName", "Store is null");
                }
                else
                {
                    return PartialView("StoreName", this.LoginStore.Name);
                }

            }
        }
        //  [OutputCache(CacheProfile = "Cache20Minutes")]
        public ActionResult StoreSearch()
        {
            if (User.Identity.IsAuthenticated)
            {
                return PartialView("StoreSearch");
            }
            else
            {
                return new EmptyResult();
            }
        }
        public ActionResult ReturnFrontEndUrl()
        {
            if (IsSuperAdmin)
            {
                return new EmptyResult();
            }
            else
            {

                if (LoginStore == null)
                {
                    return PartialView("ReturnFrontEndUrl", "");
                }
                else
                {
                    return PartialView("ReturnFrontEndUrl", this.LoginStore);
                }

            }
        }

        public ActionResult AdminSearch(String adminsearchkey, int page = 1)
        {
            if (String.IsNullOrEmpty(adminsearchkey))
            {
                return View(new PagedList<BaseEntity>(new List<BaseEntity>(), page - 1, 20, 0));
            }
            ViewBag.SearchKey = adminsearchkey;
            adminsearchkey = adminsearchkey.Trim().ToLower();

            int storeId = this.LoginStore.Id;
            String key = String.Format("SearchEntireStore-{0}-{1}", storeId, adminsearchkey);
            List<BaseEntity> resultList = null;
            StoreSearchCache.TryGet(key, out resultList);

            if (resultList == null)
            {
                resultList = SearchEntireStoreAsync(adminsearchkey, storeId).Result;
                StoreSearchCache.Set(key, resultList, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("SearchEntireStore_Minute", 10)));
            }

            var returnSearchModel = new PagedList<BaseEntity>(resultList, page - 1, 20, resultList.Count);
            return View(returnSearchModel);
        }
        public async Task<List<BaseEntity>> SearchEntireStoreAsync(String adminsearchkey, int storeId)
        {
            List<BaseEntity> res = await Task.FromResult<List<BaseEntity>>(SearchEntireStore(adminsearchkey, storeId));

            return res;
        }

        private List<BaseEntity> SearchEntireStore(string adminsearchkey, int storeId)
        {
            var resultList = new List<BaseEntity>();
            var contentList = ContentRepository.GetContentsByStoreId(storeId, adminsearchkey, StoreConstants.NewsType);
            resultList.AddRange(contentList);

            contentList = ContentRepository.GetContentsByStoreId(storeId, adminsearchkey, StoreConstants.BlogsType);
            resultList.AddRange(contentList);

            var productList = ProductRepository.GetProductsByStoreId(storeId, adminsearchkey);
            resultList.AddRange(productList);


            var brandList = BrandRepository.GetBrandsByStoreId(storeId, adminsearchkey);
            resultList.AddRange(brandList);


            var locationList = LocationRepository.GetLocationsByStoreId(storeId, adminsearchkey);
            resultList.AddRange(locationList);


            var emailList = EmailListRepository.GetStoreEmailList(storeId, adminsearchkey);
            resultList.AddRange(emailList);


            var contactsList = ContactRepository.GetContactsByStoreId(storeId, adminsearchkey);
            resultList.AddRange(contactsList);


            var categoriesList = CategoryRepository.GetCategoriesByStoreId(storeId, StoreConstants.NewsType, adminsearchkey);
            resultList.AddRange(categoriesList);

            categoriesList = CategoryRepository.GetCategoriesByStoreId(storeId, StoreConstants.BlogsType, adminsearchkey);
            resultList.AddRange(categoriesList);


            var productCategoriesList = ProductCategoryRepository.GetProductCategoriesByStoreId(storeId,
                                                                                                StoreConstants.ProductType,
                                                                                                adminsearchkey);
            resultList.AddRange(productCategoriesList);

            var settingsList = SettingRepository.GetStoreSettingsByType(storeId, "", adminsearchkey);
            resultList.AddRange(settingsList);


            var navigationList = NavigationRepository.GetNavigationsByStoreId(storeId, adminsearchkey);
            resultList.AddRange(navigationList);


            var activitiesList = ActivityRepository.GetActivitiesByStoreId(storeId, adminsearchkey);
            resultList.AddRange(activitiesList);

            return resultList;
        }

        public ActionResult SuperAdminSearch(String adminsearchkey, int page = 1)
        {
            ViewBag.SearchKey = adminsearchkey;
            adminsearchkey = adminsearchkey.Trim().ToLower();
            List<BaseContent> resultList = new List<BaseContent>();


            var contentList = from cus in this.DbContext.Contents
                              where cus.Name.ToLower().Contains(adminsearchkey)
                              orderby cus.Ordering, cus.Id descending
                              select cus;
            resultList.AddRange(contentList.ToList());

            var productList = from cus in this.DbContext.Products
                              where cus.Name.ToLower().Contains(adminsearchkey)
                              orderby cus.Ordering, cus.Id descending
                              select cus;

            resultList.AddRange(productList.ToList());
            var returnSearchModel = new PagedList<BaseContent>(resultList, page - 1, 20, resultList.Count);
            return View(returnSearchModel);
        }


        public ActionResult LabelsDropDown(int storeId = 0, String labelType = "", int[] selectedLabelsId = null)
        {
            var resultList = new List<Label>();
            storeId = GetStoreId(storeId);
            resultList = LabelRepository.GetStoreLabels(storeId);

            var items = new List<SelectListItem>();
            foreach (var label in resultList)
            {
                items.Add(new SelectListItem { Text = label.Name, Value = label.Id.ToStr(), Selected = selectedLabelsId != null && selectedLabelsId.Contains(label.Id) });
            }

            return PartialView("LabelsDropDown", items);

        }

    }
}
