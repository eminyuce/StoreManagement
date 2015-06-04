﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.Services;

namespace StoreManagement.Controllers
{
    public class ProductsController : BaseController
    {

        public ActionResult Index()
        {
            var returnModel = new ProductsViewModel();

            return View(returnModel);
        }
        //
        // GET: /Products/
        public ActionResult Product(String id)
        {
            var returnModel = new ProductDetailViewModel();
            int productId = id.Split("-".ToCharArray()).Last().ToInt();
            returnModel.Content = ContentService.GetContentsContentId(productId);
            returnModel.Store = Store;
            returnModel.Category = CategoryService.GetSingle(returnModel.Content.CategoryId);
            returnModel.Categories = CategoryService.GetCategoriesByStoreId(Store.Id, "product");
            var c = new List<Content>();
            c.Add(returnModel.Content);
            returnModel.RelatedContents = ContentService.GetContentByTypeAndCategoryId(Store.Id, "product",
                returnModel.Content.CategoryId).Except(c).Take(5).ToList();


            return View(returnModel);
        }
	}
}