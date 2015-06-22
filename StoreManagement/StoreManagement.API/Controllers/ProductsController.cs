﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StoreManagement.Data.Entities;
using StoreManagement.Data.Paging;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.API.Controllers
{
    public class ProductsController : BaseApiController<Product>, IProductService
    {
        public override IEnumerable<Product> GetAll()
        {
            return ProductRepository.GetAll();
        }

        public override Product Get(int id)
        {
            return ProductRepository.GetSingle(id);
        }

        public override HttpResponseMessage Post(Product value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Put(int id, Product value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Product GetProductsById(int productId)
        {
            return ProductRepository.GetProductsById(productId);
        }

        public List<Product> GetProductByType(string typeName)
        {
            return ProductRepository.GetProductByType(typeName);
        }

        public List<Product> GetProductByType(int storeId, string typeName)
        {
            return ProductRepository.GetProductByType(storeId, typeName);
        }

        public List<Product> GetProductByTypeAndCategoryId(int storeId, string typeName, int categoryId)
        {
            return ProductRepository.GetProductByTypeAndCategoryId(storeId, typeName, categoryId);
        }

        public List<Product> GetProductByTypeAndCategoryIdFromCache(int storeId, string typeName, int categoryId)
        {
            return ProductRepository.GetProductByTypeAndCategoryIdFromCache(storeId, typeName, categoryId);
        }

        public StorePagedList<Product> GetProductsCategoryId(int storeId, int? categoryId, string typeName, bool? isActive, int page,
                                                    int pageSize)
        {
            return ProductRepository.GetProductsCategoryId(storeId, categoryId, typeName, isActive, page, pageSize);
        }

        public Product GetProductWithFiles(int id)
        {
            return ProductRepository.GetProductWithFiles(id);
        }
    }
}