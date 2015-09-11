using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GenericRepository;
using Newtonsoft.Json.Linq;
using Ninject;
using StoreManagement.Data;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.HelpersModel;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.GenericRepositories;
using StoreManagement.Service.Repositories;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Admin.Controllers
{

    [Authorize]
    public class AjaxController : BaseController
    {
        //
        // GET: /Ajax/


        public ActionResult GetCategoriesRelatedItemsCount(int id = 0, int[] categoriesId = null, String type = "")
        {
            int storeId = GetStoreId(id);

            if (categoriesId == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                String key = String.Format("GetCategoriesRelatedItemsCount-StoreId-{0}-type-{2}-Category-{1}", id, String.Join(",", categoriesId), type);

                var result = (Dictionary<String, String>)MemoryCache.Default.Get(key);
                if (result == null)
                {
                    result = FetchCategoriesRelatedItemsCount(id, categoriesId, type);

                    CacheItemPolicy policy = null;

                    policy = new CacheItemPolicy();
                    policy.Priority = CacheItemPriority.Default;
                    policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(ProjectAppSettings.CacheMediumSeconds);

                    MemoryCache.Default.Set(key, result, policy);
                }
                return Json(result, JsonRequestBehavior.AllowGet);
           
            }
        }

        private Dictionary<String, String> FetchCategoriesRelatedItemsCount(int id, int[] categoriesId, string type)
        {
            var result = new Dictionary<String, String>();
            if (type.Equals(StoreConstants.ProductType))
            {
                foreach (var catId in categoriesId)
                {
                    int categoryId = catId;
                    var cnt = this.ProductRepository.Count(r => r.StoreId == id && r.ProductCategoryId == categoryId);
                    result[categoryId.ToStr()] = cnt.ToStr();
                }
            }
            else if (type.Equals(StoreConstants.BlogsType, StringComparison.InvariantCultureIgnoreCase) ||
                     type.Equals(StoreConstants.NewsType, StringComparison.InvariantCultureIgnoreCase))
            {
                foreach (var catId in categoriesId)
                {
                    int categoryId = catId;
                    var cnt =
                        this.ContentRepository.Count(
                            r =>
                            r.StoreId == id && r.CategoryId == categoryId &&
                            r.Type.Equals(type, StringComparison.InvariantCultureIgnoreCase));
                    result[categoryId.ToStr()] = cnt.ToStr();
                }
            }

            return result;

        }

        public ActionResult SearchAutoComplete(String term, String action, String controller, int id = 0)
        {
            int storeId = GetStoreId(id);
            var list = new List<String>();
            String searchKey = term;
            if (action.Equals("Index", StringComparison.InvariantCultureIgnoreCase) &&
                    controller.Equals("Products", StringComparison.InvariantCultureIgnoreCase))
            {
                list = ProductRepository.GetProductsByStoreId(storeId, searchKey).Select(r => r.Name).ToList();
            }
            else if (action.Equals("Index", StringComparison.InvariantCultureIgnoreCase) &&
                    controller.Equals("News", StringComparison.InvariantCultureIgnoreCase))
            {
                list = ContentRepository.GetContentsByStoreId(storeId, searchKey, StoreConstants.NewsType).Select(r => r.Name).ToList();
            }
            else if (action.Equals("Index", StringComparison.InvariantCultureIgnoreCase) &&
                    controller.Equals("Blogs", StringComparison.InvariantCultureIgnoreCase))
            {
                list = ContentRepository.GetContentsByStoreId(storeId, searchKey, StoreConstants.BlogsType).Select(r => r.Name).ToList();
            }
            else if (action.Equals("Index", StringComparison.InvariantCultureIgnoreCase) &&
                    controller.Equals("Navigations", StringComparison.InvariantCultureIgnoreCase))
            {
                list = NavigationRepository.GetNavigationsByStoreId(storeId, searchKey).Select(r => r.Name).ToList();
            }
            else if (action.Equals("Index", StringComparison.InvariantCultureIgnoreCase) &&
                    controller.Equals("Stores", StringComparison.InvariantCultureIgnoreCase))
            {
                list = StoreRepository.GetStoresByStoreId(searchKey).Select(r => r.Name).ToList();
            }
            else if (action.Equals("Index", StringComparison.InvariantCultureIgnoreCase) &&
                  controller.Equals("Labels", StringComparison.InvariantCultureIgnoreCase))
            {
                list = LabelRepository.GetLabelsByStoreId(storeId, searchKey).Select(r => r.Name).ToList();
            }
            else if (action.Equals("Index", StringComparison.InvariantCultureIgnoreCase) &&
                 controller.Equals("EmailLists", StringComparison.InvariantCultureIgnoreCase))
            {
                list = EmailListRepository.GetStoreEmailList(storeId, searchKey).Select(r => r.Email).ToList();
            }
            else if (action.Equals("Index", StringComparison.InvariantCultureIgnoreCase) &&
               controller.Equals("Contacts", StringComparison.InvariantCultureIgnoreCase))
            {
                list = ContactRepository.GetContactsByStoreId(storeId, searchKey).Select(r => r.Name).ToList();
            }
            else if (action.Equals("Index", StringComparison.InvariantCultureIgnoreCase) &&
               controller.Equals("Locations", StringComparison.InvariantCultureIgnoreCase))
            {
                list = LocationRepository.GetLocationsByStoreId(storeId, searchKey).Select(r => r.Address).ToList();
            }
            else if (action.Equals("Index", StringComparison.InvariantCultureIgnoreCase) &&
               controller.Equals("Brands", StringComparison.InvariantCultureIgnoreCase))
            {
                list = BrandRepository.GetBrandsByStoreId(storeId, searchKey).Select(r => r.Name).ToList();
            }
            else if (action.Equals("Index", StringComparison.InvariantCultureIgnoreCase) &&
             controller.Equals("ProductCategories", StringComparison.InvariantCultureIgnoreCase))
            {
                list = ProductCategoryRepository.GetProductCategoriesByStoreId(storeId, StoreConstants.ProductType, searchKey).Select(r => r.Name).ToList();
            }
            else if (action.Equals("Index", StringComparison.InvariantCultureIgnoreCase) &&
                controller.Equals("BlogsCategories", StringComparison.InvariantCultureIgnoreCase))
            {
                list = CategoryRepository.GetCategoriesByStoreId(storeId, StoreConstants.BlogsType, searchKey).Select(r => r.Name).ToList();
            }
            else if (action.Equals("Index", StringComparison.InvariantCultureIgnoreCase) &&
                    controller.Equals("NewsCategories", StringComparison.InvariantCultureIgnoreCase))
            {
                list = CategoryRepository.GetCategoriesByStoreId(storeId, StoreConstants.NewsType, searchKey).Select(r => r.Name).ToList();
            }
            else if (action.Equals("Index", StringComparison.InvariantCultureIgnoreCase) &&
                   controller.Equals("StoreCategories", StringComparison.InvariantCultureIgnoreCase))
            {
                list = CategoryRepository.GetCategoriesByStoreId(0, StoreConstants.StoreType, searchKey).Select(r => r.Name).ToList();
            }
            else if (action.Equals("Index", StringComparison.InvariantCultureIgnoreCase) &&
                controller.Equals("PageDesigns", StringComparison.InvariantCultureIgnoreCase))
            {
                list = PageDesignRepository.GetPageDesignByStoreId(storeId, searchKey).Select(r => r.Name).ToList();
            }
            else if (action.Equals("DisplayImages", StringComparison.InvariantCultureIgnoreCase) &&
              controller.Equals("FileManager", StringComparison.InvariantCultureIgnoreCase))
            {
                list = FileManagerRepository.GetFilesBySearchKey(storeId, searchKey).Select(r => r.OriginalFilename).ToList();
            }
            else if (action.Equals("Settings", StringComparison.InvariantCultureIgnoreCase) &&
                  controller.Equals("Stores", StringComparison.InvariantCultureIgnoreCase))
            {
                list = SettingRepository.GetStoreSettingsByType(storeId, "", searchKey).Select(r => String.Format("{0}", r.SettingKey)).ToList();
            }
            else if (action.Equals("Index", StringComparison.InvariantCultureIgnoreCase) &&
                 controller.Equals("StoreLanguages", StringComparison.InvariantCultureIgnoreCase))
            {
                list = StoreLanguageRepository.GetStoreLanguages(storeId, searchKey).Select(r => String.Format("{0}", r.Name)).ToList();
            }
            else if (action.Equals("Index", StringComparison.InvariantCultureIgnoreCase) &&
                 controller.Equals("Activities", StringComparison.InvariantCultureIgnoreCase))
            {
                list = ActivityRepository.GetActivitiesByStoreId(storeId, searchKey).Select(r => String.Format("{0}", r.Name)).ToList();
            }


            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetStoreLabels(int storeId)
        {
            var labels = LabelRepository.GetActiveLabels(storeId);
            return Json(labels, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddFileManagerLabels(String[] labels, List<String> selectedFiles, int storeId)
        {
            // List<FileManager> files = FileManagerRepository.GetFilesByGoogleImageIdArray(selectedFiles.ToArray());
            var isNewLabelExists = SaveImagesLabels(labels, selectedFiles, storeId);
            return Json(isNewLabelExists, JsonRequestBehavior.AllowGet);
        }




        public ActionResult CreatingNewLabel(String labelName)
        {

            Label label = new Label();
            label.Name = labelName;
            label.ParentId = 1;

            LabelRepository.Add(label);
            int labelId = LabelRepository.Save();
            label = LabelRepository.GetSingle(labelId);


            return Json(label, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SetSettings(List<Setting> settings, int storeId)
        {
            foreach (Setting v in settings)
            {

                try
                {

                    var item = new Setting();
                    if (v.Id == 0)
                    {
                        item.SettingKey = v.SettingKey;
                        item.SettingValue = v.SettingValue;
                        item.CreatedDate = DateTime.Now;
                        item.State = true;
                        item.StoreId = storeId;
                        item.UpdatedDate = DateTime.Now;
                        item.Name = "";
                        item.Description = "";
                        item.Type = "StoreSettings";
                        item.Ordering = 1;
                        SettingRepository.Add(item);
                        SettingRepository.Save();
                    }
                    else
                    {
                        item = SettingRepository.GetSingle(v.Id);
                        item.SettingValue = v.SettingValue;
                        item.State = true;
                        item.StoreId = storeId;
                        item.UpdatedDate = DateTime.Now;
                        item.Name = "";
                        item.Description = "";
                        item.Type = "StoreSettings";
                        item.Ordering = 1;
                        SettingRepository.Edit(item);
                        SettingRepository.Save();
                    }



                }
                catch (DbEntityValidationException ex)
                {
                    var message = GetDbEntityValidationExceptionDetail(ex);
                    Logger.Error(ex, "DbEntityValidationException:" + message, v);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Exception is occured while saving settings:", v);

                }

            }


            return Json(true, JsonRequestBehavior.AllowGet);
        }



        public ActionResult ChangeIsCarouselState(int fileId = 0, bool isCarousel = false)
        {
            try
            {


                var s = FileManagerRepository.GetSingle(fileId);
                s.IsCarousel = isCarousel;
                FileManagerRepository.Edit(s);
                FileManagerRepository.Save();

            }
            catch (DbEntityValidationException ex)
            {
                var message = GetDbEntityValidationExceptionDetail(ex);
                Logger.Error(ex, "DbEntityValidationException:" + message);
            }
            catch (Exception exception)
            {
                Logger.ErrorException("ChangeIsCarouselState  fileId" + fileId + " isCarousel:" + isCarousel, exception);
            }

            return Json(new { fileId, isCarousel }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteEmailListGridItem(List<String> values)
        {
            BaseEntityRepository.DeleteBaseEntity(EmailListRepository, values);
            return Json(values, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteSettingGridItem(List<String> values)
        {
            BaseEntityRepository.DeleteBaseEntity(SettingRepository, values);
            return Json(values, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeletePageDesignsGridItem(List<String> values)
        {
            BaseEntityRepository.DeleteBaseEntity(PageDesignRepository, values);
            return Json(values, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteStoreLanguageGridItem(List<String> values)
        {
            BaseEntityRepository.DeleteBaseEntity(StoreLanguageRepository, values);
            return Json(values, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteNavigationGridItem(List<String> values)
        {
            BaseEntityRepository.DeleteBaseEntity(NavigationRepository, values);
            return Json(values, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteProductGridItem(List<String> values)
        {
            BaseEntityRepository.DeleteBaseEntity(ProductRepository, values);
            return Json(values, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteLocationsGridItem(List<String> values)
        {
            BaseEntityRepository.DeleteBaseEntity(LocationRepository, values);
            return Json(values, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteContactsGridItem(List<String> values)
        {
            BaseEntityRepository.DeleteBaseEntity(ContactRepository, values);
            return Json(values, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteFileManagerGridItem(List<String> values)
        {
            try
            {
                List<String> googleIdList = new List<string>();
                List<int> idList = new List<int>();
                foreach (var value in values)
                {
                    String googleId = value.Split("-".ToCharArray())[0];
                    String id = value.Split("-".ToCharArray())[1];

                    googleIdList.Add(googleId);
                    idList.Add(id.ToInt());
                }
                var item = FileManagerRepository.GetSingle(idList.FirstOrDefault());
                ConnectToStoreGoogleDrive(item.StoreId);

                foreach (String v in values)
                {
                    string googledriveFileId = v;
                    Task.Run(() => DeleteFile(googledriveFileId));
                    Thread.Sleep(50);
                }
            }
            catch (DbEntityValidationException ex)
            {
                var message = GetDbEntityValidationExceptionDetail(ex);
                Logger.Error(ex, "DbEntityValidationException:" + message);
            }
            catch (Exception exception)
            {
                Logger.ErrorException("DeleteFileManagerGridItem :" + String.Join(",", values), exception);
            }
            return Json(values, JsonRequestBehavior.AllowGet);
        }

        private void DeleteFile(string googledriveFileId)
        {
            String id = "";
            try
            {
                String googleId = googledriveFileId.Split("-".ToCharArray())[0];
                id = googledriveFileId.Split("-".ToCharArray())[1];

                if (!String.IsNullOrEmpty(googleId))
                {
                    try
                    {

                        UploadHelper.deleteFile(googleId);
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorException(String.Format("GoogleId={0} file could not deleted from google drive", googledriveFileId), ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorException(String.Format("GoogleId={0} could not deleted from DB.", googledriveFileId), ex);
            }

            try
            {
                var item = FileManagerRepository.GetSingle(id.ToInt());
                FileManagerRepository.Delete(item);
                FileManagerRepository.Save();
            }
            catch (DbEntityValidationException ex)
            {
                var message = GetDbEntityValidationExceptionDetail(ex);
                Logger.Error(ex, "DbEntityValidationException:" + message);
            }
            catch (Exception ex)
            {
                Logger.ErrorException(String.Format("GoogleId={0} could not deleted from DB.", googledriveFileId), ex);
            }
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteBrandGridItem(List<String> values)
        {
            BaseEntityRepository.DeleteBaseEntity(BrandRepository, values);
            return Json(values, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteCategoryGridItem(List<String> values)
        {
            BaseEntityRepository.DeleteBaseEntity(CategoryRepository, values);
            return Json(values, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteProductCategoryGridItem(List<String> values)
        {
            BaseEntityRepository.DeleteBaseEntity(ProductCategoryRepository, values);
            return Json(values, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteActivityGridItem(List<String> values)
        {
            BaseEntityRepository.DeleteBaseEntity(ActivityRepository, values);
            return Json(values, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteContentGridItem(List<String> values)
        {
            BaseEntityRepository.DeleteBaseEntity(ContentRepository, values);
            return Json(values, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeEmailListGridOrderingOrState(List<OrderingItem> values, String checkbox = "")
        {
            BaseEntityRepository.ChangeGridBaseEntityOrderingOrState(EmailListRepository, values, checkbox);
            return Json(new { values, checkbox }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeProductCategoryGridOrderingOrState(List<OrderingItem> values, String checkbox = "")
        {
            BaseEntityRepository.ChangeGridBaseEntityOrderingOrState(ProductCategoryRepository, values, checkbox);
            return Json(new { values, checkbox }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeLabelGridOrderingOrState(List<OrderingItem> values, String checkbox = "")
        {
            BaseEntityRepository.ChangeGridBaseEntityOrderingOrState(LabelRepository, values, checkbox);
            return Json(new { values, checkbox }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeCategoryGridOrderingOrState(List<OrderingItem> values, String checkbox = "")
        {
            BaseEntityRepository.ChangeGridBaseEntityOrderingOrState(CategoryRepository, values, checkbox);
            return Json(new { values, checkbox }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeBrandGridOrderingOrState(List<OrderingItem> values, String checkbox = "")
        {
            BaseEntityRepository.ChangeGridBaseEntityOrderingOrState(BrandRepository, values, checkbox);
            return Json(new { values, checkbox }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeLocationsGridOrderingOrState(List<OrderingItem> values, String checkbox = "")
        {
            BaseEntityRepository.ChangeGridBaseEntityOrderingOrState(LocationRepository, values, checkbox);
            return Json(new { values, checkbox }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeContactsGridOrderingOrState(List<OrderingItem> values, String checkbox = "")
        {
            BaseEntityRepository.ChangeGridBaseEntityOrderingOrState(ContactRepository, values, checkbox);
            return Json(new { values, checkbox }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangePageDesignsGridOrderingOrState(List<OrderingItem> values, String checkbox = "")
        {
            BaseEntityRepository.ChangeGridBaseEntityOrderingOrState(PageDesignRepository, values, checkbox);
            return Json(new { values, checkbox }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeStoreLanguageGridOrderingOrState(List<OrderingItem> values, String checkbox = "")
        {
            BaseEntityRepository.ChangeGridBaseEntityOrderingOrState(StoreLanguageRepository, values, checkbox);
            return Json(new { values, checkbox }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeNavigationGridOrderingOrState(List<OrderingItem> values, String checkbox = "")
        {
            BaseEntityRepository.ChangeGridBaseEntityOrderingOrState(NavigationRepository, values, checkbox);
            return Json(new { values, checkbox }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeFileManagerGridOrderingOrState(List<OrderingItem> values, String checkbox = "")
        {
            try
            {
                foreach (OrderingItem item in values)
                {
                    var content = FileManagerRepository.GetSingle(item.Id);
                    if (String.IsNullOrEmpty(checkbox))
                    {
                        content.Ordering = item.Ordering;
                    }
                    else if (checkbox.Equals("state", StringComparison.InvariantCultureIgnoreCase))
                    {
                        content.State = item.State;
                    }
                    else if (checkbox.Equals("Carousel", StringComparison.InvariantCultureIgnoreCase))
                    {
                        content.IsCarousel = item.State;
                    }

                    FileManagerRepository.Edit(content);
                }
                FileManagerRepository.Save();
            }
            catch (Exception exception)
            {
                Logger.ErrorException("ChangeFileManagerGridOrderingOrState :" + String.Join(",", values), exception);
            }
            return Json(new { values, checkbox }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeContentGridOrderingOrState(List<OrderingItem> values, String checkbox = "")
        {
            BaseEntityRepository.ChangeGridBaseContentOrderingOrState(ContentRepository, values, checkbox);
            return Json(new { values, checkbox }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeActivityGridOrderingOrState(List<OrderingItem> values, String checkbox = "")
        {
            BaseEntityRepository.ChangeGridBaseEntityOrderingOrState(ActivityRepository, values, checkbox);
            return Json(new { values, checkbox }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeProductGridOrderingOrState(List<OrderingItem> values, String checkbox = "")
        {
            BaseEntityRepository.ChangeGridBaseContentOrderingOrState(ProductRepository, values, checkbox);
            return Json(new { values, checkbox }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveStyles(int storeId = 0, String styleArray = "")
        {
            try
            {

                JObject results = JObject.Parse(styleArray);
                foreach (var result in results["styleArray"])
                {
                    string id = (string)result["Id"];
                    string style = (string)result["Style"];
                    var s = this.SettingRepository.GetSingle(id.ToInt());
                    s.SettingValue = style;
                    SettingRepository.Edit(s);
                }
                SettingRepository.Save();

            }
            catch (Exception exception)
            {
                Logger.ErrorException("SaveStyles :" + storeId + " styleArray:" + styleArray, exception);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRootCategories(int storeId = 0)
        {
            var cat = CategoryRepository.FindBy(r => r.ParentId == 0 && r.StoreId == storeId).ToList();
            var returnJson = from c in cat select new { Text = c.Name, Value = c.Id };
            return Json(returnJson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRootProductCategories(int storeId = 0)
        {
            var cat = ProductCategoryRepository.FindBy(r => r.ParentId == 0 && r.StoreId == storeId).ToList();
            var returnJson = from c in cat select new { Text = c.Name, Value = c.Id };
            return Json(returnJson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveSettingValue(int id = 0, string value = "")
        {
            var s = SettingRepository.GetSingle(id);
            s.SettingValue = value;
            SettingRepository.Edit(s);
            SettingRepository.Save();
            return Content(value);
        }

        public ActionResult GetImagesByLabels(int storeId, String[] labels, String entityType, int id)
        {
            var images = FileManagerRepository.GetFilesByStoreIdAndLabels(storeId, labels);
            var list = new List<FileManager>();
            if (entityType.Equals(StoreConstants.ProductType, StringComparison.InvariantCultureIgnoreCase))
            {
                var productFiles = ProductFileRepository.GetProductFilesByProductId(id);
                list.AddRange(images.Where(r => !productFiles.Select(r1 => r1.FileManagerId).Contains(r.Id)));

            }
            else if (entityType.Equals(StoreConstants.BlogsType, StringComparison.InvariantCultureIgnoreCase) || entityType.Equals(StoreConstants.NewsType, StringComparison.InvariantCultureIgnoreCase))
            {
                var contentFiles = ContentFileRepository.GetContentFilesByContentId(id);
                list.AddRange(images.Where(r => !contentFiles.Select(r1 => r1.FileManagerId).Contains(r.Id)));
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SetMainImage(int fileId, int id, String entityType)
        {

            if (entityType.Equals(StoreConstants.ProductType, StringComparison.InvariantCultureIgnoreCase))
            {
                ProductFileRepository.SetMainImage(id, fileId);
            }
            else if (entityType.Equals(StoreConstants.BlogsType, StringComparison.InvariantCultureIgnoreCase) || entityType.Equals(StoreConstants.NewsType, StringComparison.InvariantCultureIgnoreCase))
            {
                ContentFileRepository.SetMainImage(id, fileId);
            }

            return Json(new { fileId, id }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetImages(int storeId)
        {
            var images = FileManagerRepository.GetFilesByStoreId(storeId);
            return Json(images, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetHiearchicalNodesInfo()
        {
            var tree = this.CategoryRepository.GetCategoriesByStoreId(1, "family");

            return Json(tree, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetFiles(int contentId)
        {
            var files = ContentFileRepository.GetContentFilesByContentId(contentId);
            return Json(files, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCategoriesTree(int storeId = 0, String categoryType = "")
        {
            var tree = new List<BaseCategory>(this.CategoryRepository.GetCategoriesByStoreId(storeId, categoryType));

            var html = this.RenderPartialToString(
                        "pCreateCategoryTree",
                        new ViewDataDictionary(tree), null);

            return Json(html, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProductCategoriesTree(int storeId = 0, String categoryType = "")
        {
            var tree = new List<BaseCategory>(this.ProductCategoryRepository.GetProductCategoriesByStoreId(storeId, categoryType));

            var html = this.RenderPartialToString(
                        "pCreateCategoryTree",
                        new ViewDataDictionary(tree), null);

            return Json(html, JsonRequestBehavior.AllowGet);
        }


        private SelectList GetModuls(int storeId = 0)
        {
            var moduls = GetDefaultModuls();
            storeId = GetStoreId(storeId);
            var navigations = NavigationRepository.GetStoreNavigations(storeId);


            foreach (var navigation in navigations)
            {
                String value = navigation.ControllerName.ToLower() + "-" + navigation.ActionName.ToLower();

                if (moduls.Any(r => value.Equals(r.Value.ToLower(), StringComparison.InvariantCultureIgnoreCase)))
                {
                    moduls.Remove(
                        moduls.FirstOrDefault(
                            r => r.Value.ToLower().Equals(value, StringComparison.InvariantCultureIgnoreCase)));
                }
            }

            var m = new SelectListItem();
            m.Value = "Pages-Pages";
            m.Text = "Pages";
            moduls.Add(m);

            var sList = new SelectList(moduls, "Value", "Text");
            return sList;
        }

        private static List<SelectListItem> GetDefaultModuls()
        {
            var moduls = new List<SelectListItem>();
            var m = new SelectListItem();
            m.Value = "Home-Index";
            m.Text = "Home";
            moduls.Add(m);
            m = new SelectListItem();
            m.Value = "News-Index";
            m.Text = "News";
            moduls.Add(m);
            m = new SelectListItem();
            m.Value = "Products-Index";
            m.Text = "Products";
            moduls.Add(m);
            m = new SelectListItem();
            m.Value = "Blogs-Index";
            m.Text = "Blogs";
            moduls.Add(m);
            m = new SelectListItem();
            m.Value = "Activities-Index";
            m.Text = "Activities";
            moduls.Add(m);
            m = new SelectListItem();
            m.Value = "Locations-Index";
            m.Text = "Locations";
            moduls.Add(m);
            m = new SelectListItem();
            m.Value = "Contacts-Index";
            m.Text = "Contacts";
            moduls.Add(m);
            m = new SelectListItem();
            m.Value = "Photos-Index";
            m.Text = "Photo Gallery";
            moduls.Add(m);
            return moduls;
        }
        public ActionResult GetStoreModuls(int id)
        {
            int storeId = id;
            var moduls = GetModuls(storeId);
            return Json(moduls, JsonRequestBehavior.AllowGet);
        }



    }

}
