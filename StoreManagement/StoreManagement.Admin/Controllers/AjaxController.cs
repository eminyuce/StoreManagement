﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Ninject;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Admin.Controllers
{

    [Authorize]
    public class AjaxController : BaseController
    {
        //
        // GET: /Ajax/

        public class OrderingItem
        {
            public int Id { get; set; }
            public int Ordering { get; set; }
            public bool State { get; set; }

        }

        [HttpPost]
        public ActionResult SetSettings(List<Setting> settings, int storeId)
        {
            foreach (Setting v in settings)
            {
                var item = new Setting();
                if (v.Id == 0)
                {
                    item.SettingKey = v.SettingKey;
                    item.CreatedDate = DateTime.Now;
                    SettingRepository.Add(item);
                }
                else
                {
                    item = SettingRepository.GetSingle(v.Id);
                    SettingRepository.Edit(item);
                }
                item.SettingValue = v.SettingValue;
                item.State = true;
                item.StoreId = storeId;
                item.UpdatedDate = DateTime.Now;
                item.Name = "";
                item.Description = "";
                item.Type = "StoreSettings";

            }
            SettingRepository.Save();

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeIsCarouselState(int fileId = 0, bool isCarousel = false)
        {
            var s = FileManagerRepository.GetSingle(fileId);
            s.IsCarousel = isCarousel;
            FileManagerRepository.Edit(s);
            FileManagerRepository.Save();
            return Json(new { fileId, isCarousel }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteEmailListGridItem(List<String> values)
        {
            foreach (String v in values)
            {
                var id = v.ToInt();
                var item = EmailListRepository.GetSingle(id);
                EmailListRepository.Delete(item);
            }
            EmailListRepository.Save();

            return Json(values, JsonRequestBehavior.AllowGet);
        }
    
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteSettingGridItem(List<String> values)
        {
            foreach (String v in values)
            {
                var id = v.ToInt();
                var item = SettingRepository.GetSingle(id);
                SettingRepository.Delete(item);
            }
            SettingRepository.Save();

            return Json(values, JsonRequestBehavior.AllowGet);
        } 
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteNavigationGridItem(List<String> values)
        {
            foreach (String v in values)
            {
                var id = v.ToInt();
                var item = NavigationRepository.GetSingle(id);
                NavigationRepository.Delete(item);
            }
            NavigationRepository.Save();

            return Json(values, JsonRequestBehavior.AllowGet);
        } 
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteProductGridItem(List<String> values)
        {
            foreach (String v in values)
            {
                var id = v.ToInt();
                var item = ProductRepository.GetSingle(id);
                ProductRepository.Delete(item);
            }
            ProductRepository.Save();
            
            return Json(values, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteCategoryGridItem(List<String> values)
        {
            foreach (String v in values)
            {
                var id = v.ToInt();
                var item =  CategoryRepository.GetSingle(id);
                CategoryRepository.Delete(item);
            }
            CategoryRepository.Save();
            return Json(values, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteProductCategoryGridItem(List<String> values)
        {
            foreach (String v in values)
            {
                var id = v.ToInt();
                var item = ProductCategoryRepository.GetProductCategory(id);
                ProductCategoryRepository.Delete(item);
            }
            ProductCategoryRepository.Save();
            
            return Json(values, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin,StoreAdmin")]
        public ActionResult DeleteContentGridItem(List<String> values)
        {
            foreach (String id in values)
            {
                var contentId = id.ToInt();
                var content = ContentRepository.GetSingle(contentId);
                ContentRepository.Delete(content);
            }
            ContentRepository.Save();

            return Json(values, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeEmailListGridOrderingOrState(List<OrderingItem> values, String checkbox = "")
        {
            foreach (OrderingItem item in values)
            {
                var nav = EmailListRepository.GetSingle(item.Id);
                if (String.IsNullOrEmpty(checkbox))
                {
                    nav.Ordering = item.Ordering;
                }
                if (checkbox.Equals("state", StringComparison.InvariantCultureIgnoreCase))
                {
                    nav.State = item.State;
                }

                EmailListRepository.Edit(nav);
            }
            EmailListRepository.Save();
            return Json(new { values, checkbox }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeProductCategoryGridOrderingOrState(List<OrderingItem> values, String checkbox = "")
        {
            foreach (OrderingItem item in values)
            {
                var nav = ProductCategoryRepository.GetProductCategory(item.Id);
                if (String.IsNullOrEmpty(checkbox))
                {
                    nav.Ordering = item.Ordering;
                }
                if (checkbox.Equals("state", StringComparison.InvariantCultureIgnoreCase))
                {
                    nav.State = item.State;
                }

                ProductCategoryRepository.Edit(nav);
            }
            ProductCategoryRepository.Save();
            return Json(new { values, checkbox }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeLabelGridOrderingOrState(List<OrderingItem> values, String checkbox = "")
        {
            foreach (OrderingItem item in values)
            {
                var nav = LabelRepository.GetSingle(item.Id);
                if (String.IsNullOrEmpty(checkbox))
                {
                    nav.Ordering = item.Ordering;
                }
                if (checkbox.Equals("state", StringComparison.InvariantCultureIgnoreCase))
                {
                    nav.State = item.State;
                }

                LabelRepository.Edit(nav);
            }
            LabelRepository.Save();
            return Json(new { values, checkbox }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeCategoryGridOrderingOrState(List<OrderingItem> values, String checkbox = "")
        {
            foreach (OrderingItem item in values)
            {
                var nav = CategoryRepository.GetSingle(item.Id);
                if (String.IsNullOrEmpty(checkbox))
                {
                    nav.Ordering = item.Ordering;
                }
                if (checkbox.Equals("state", StringComparison.InvariantCultureIgnoreCase))
                {
                    nav.State = item.State;
                }

                CategoryRepository.Edit(nav);
            }
            CategoryRepository.Save();
            return Json(new { values, checkbox }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeNavigationGridOrderingOrState(List<OrderingItem> values, String checkbox = "")
        {
            foreach (OrderingItem item in values)
            {
                var nav = NavigationRepository.GetSingle(item.Id);
                if (String.IsNullOrEmpty(checkbox))
                {
                    nav.Ordering = item.Ordering;
                }
                if (checkbox.Equals("state", StringComparison.InvariantCultureIgnoreCase))
                {
                    nav.State = item.State;
                }

                NavigationRepository.Edit(nav);
            }
            NavigationRepository.Save();
            return Json(new { values, checkbox }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeFileManagerGridOrderingOrState(List<OrderingItem> values, String checkbox = "")
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
            return Json(new { values, checkbox }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeContentGridOrderingOrState(List<OrderingItem> values, String checkbox = "")
        {
            foreach (OrderingItem item in values)
            {
                var content = ContentRepository.GetSingle(item.Id);
                if (String.IsNullOrEmpty(checkbox))
                {
                    content.Ordering = item.Ordering;
                }
                else if (checkbox.Equals("imagestate", StringComparison.InvariantCultureIgnoreCase))
                {
                    content.ImageState = item.State;
                }
                else if (checkbox.Equals("state", StringComparison.InvariantCultureIgnoreCase))
                {
                    content.State = item.State;
                }
                else if (checkbox.Equals("mainpage", StringComparison.InvariantCultureIgnoreCase))
                {
                    content.MainPage = item.State;
                }


                ContentRepository.Edit(content);
            }
            ContentRepository.Save();
            return Json(new { values, checkbox }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeProductGridOrderingOrState(List<OrderingItem> values, String checkbox = "")
        {
            foreach (OrderingItem item in values)
            {
                var product = ProductRepository.GetSingle(item.Id);
                if (String.IsNullOrEmpty(checkbox))
                {
                    product.Ordering = item.Ordering;
                }
                else if (checkbox.Equals("imagestate", StringComparison.InvariantCultureIgnoreCase))
                {
                    product.ImageState = item.State;
                }
                else if (checkbox.Equals("state", StringComparison.InvariantCultureIgnoreCase))
                {
                    product.State = item.State;
                }
                else if (checkbox.Equals("mainpage", StringComparison.InvariantCultureIgnoreCase))
                {
                    product.MainPage = item.State;
                }


                ProductRepository.Edit(product);
            }
            ProductRepository.Save();
            return Json(new { values, checkbox }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveStyles(int storeId = 0, String styleArray = "")
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
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRootCategories(int storeId = 0)
        {
             var cat = CategoryRepository.FindBy(r => r.ParentId == 0 && r.StoreId == storeId).ToList() ;
            var returnJson = from c in cat select new { Text = c.Name, Value = c.Id };
            return Json(returnJson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRootProductCategories(int storeId = 0)
        {
            var cat =  ProductCategoryRepository.FindBy(r => r.ParentId == 0 && r.StoreId == storeId).ToList() ;
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

        public ActionResult GetContentImages(int contentId)
        {
            var images = ContentFileRepository.GetContentByContentId(contentId).Select(r => r.FileManager);
            return Json(images, JsonRequestBehavior.AllowGet);
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
            var files = ContentFileRepository.GetContentByContentId(contentId);
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
            var tree =  new List<BaseCategory>(this.ProductCategoryRepository.GetProductCategoriesByStoreId(storeId, categoryType));

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
            m.Value = "Pages";
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
            m.Value = "Events-Index";
            m.Text = "Events";
            moduls.Add(m);
            m = new SelectListItem();
            m.Value = "Home-Contact";
            m.Text = "Contact";
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
