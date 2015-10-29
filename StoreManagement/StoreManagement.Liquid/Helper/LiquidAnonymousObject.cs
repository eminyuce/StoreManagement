using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Liquid.Helper
{
    public class LiquidAnonymousObject
    {

        public static IEnumerable GetFileManagerLiquidEnumerable(List<FileManagerLiquid> cats)
        {
            return from s in cats

                   select new
                   {
                       Name = s.FileManager.OriginalFilename,
                       s.ImageSource,
                       s.FileManager.FileSize
                   };
        }
        public static IEnumerable GetActivitiesEnumerable(List<ActivitiesLiquid> items)
        {
            return from s in items
                   select new
                   {
                       s.Activity.Name,
                       s.Activity.BeginDate,
                       s.Activity.Description,
                       s.Activity.FinishDate,
                       ActivityId = s.Activity.Id

                   };
        }
        public static IEnumerable GetCommentsEnumerable(List<CommentLiquid> items)
        {
            return from s in items
                   select new
                   {
                       s.Comment.Name,
                       s.Comment.Email
                   };
        }
        public static IEnumerable GetBrandsEnumerable(List<BrandLiquid> items)
        {
            return from s in items
                   select new
                   {
                       s.Brand.Name,
                       s.Brand.Description,
                       s.DetailLink
                   };
        }
        public static IEnumerable GetContactEnumerable(List<ContactLiquid> items)
        {
            var  refobj = from s in items
                   select new
                   {
                       s.Contact.Name,
                       s.Contact.Title,
                       s.Contact.PhoneCell,
                       s.Contact.PhoneWork,
                       s.Contact.Email
                   };


            return refobj;
        }
        public static IEnumerable GetLabelsEnumerable(List<LabelLiquid> items)
        {
            return from s in items
                   select new
                   {
                       s.Label.Name,
                       s.Link
                   };
        }
        public static IEnumerable GetRetailersEnumerable(List<RetailerLiquid> items)
        {
            return from s in items
                   select new
                   {
                       s.Retailer.Name,
                       s.Link,
                       s.DetailLink
                   };
        }
        public static IEnumerable GetLocationsEnumerable(List<LocationLiquid> items)
        {
            return from s in items
                   select new
                   {
                       s.Location.Name,
                       s.Location.Longitude,
                       s.Location.Latitude,
                       s.Location.State,
                       s.Location.City,
                       s.Location.Country,
                       s.Location.Address,
                       s.Location.Postal,

                   };
        }
        public static IEnumerable GetNavigationsEnumerable(List<NavigationLiquid> items)
        {
            return from s in items
                   select new
                   {
                       s.Navigation.Name,
                       s.Navigation.Static,
                       s.Link
                   };
        }
        public static IEnumerable GetProductsLiquid(List<ProductLiquid> productLiquidList)
        {
            return from s in productLiquidList
                   select new
                   {
                       CategoryName = s.Category.Name,
                       CategoryDescription = s.Category.Description,
                       CategoryId = s.Product.ProductCategoryId,
                       s.Product.BrandId,
                       ProductId = s.Product.Id,
                       s.Product.Name,
                       s.Product.Description,
                       s.Product.ProductCode,
                       s.Product.RetailerId,
                       s.Product.Price,
                       s.Product.Discount,
                       s.Product.UpdatedDate,
                       s.Product.CreatedDate,
                       s.Product.TotalRating,
                       s.Product.UnitsInStock,
                       s.Product.VideoUrl,
                       s.DetailLink,
                       s.PlainDescription,
                       images = s.ImageLiquid

                   };
        }
        public static object GetProductAnonymousObject(ProductLiquid s)
        {
            object anonymousObject = new
            {
                CategoryName = s.Category.Name,
                CategoryId = s.Product.ProductCategoryId,
                s.Product.BrandId,
                CategoryDescription = s.Category.Description,
                ProductId = s.Product.Id,
                s.Product.Name,
                s.Product.Description,
                s.Product.ProductCode,
                s.Product.Price,
                s.Product.RetailerId,
                s.Product.Discount,
                s.Product.UpdatedDate,
                s.Product.CreatedDate,
                s.Product.TotalRating,
                s.Product.UnitsInStock,
                s.Product.VideoUrl,
                images = s.ImageLiquid
            };
            return anonymousObject;
        }
        public static IEnumerable GetProductCategories(List<ProductCategoryLiquid> cats)
        {
            return from s in cats
                   select new
                   {
                       s.ProductCategory.Name,
                       s.ProductCategory.Description,
                       s.DetailLink,
                       s.Count
                   };
        }
        
        public static object GetBrandLiquid(BrandLiquid brandLiquid)
        {
            return new
            {
                brandLiquid,
                Name = brandLiquid.Brand.Name,
                Description = brandLiquid.Brand.Description
            };
        }
        public static IEnumerable GetSliderImagesLiquidList(List<FileManagerLiquid> sliderImagesLiquid)
        {
            return from s in sliderImagesLiquid
                   select new
                   {
                       imagesource = s.ImageSource,
                       s.FileManager.Title,
                       s.FileManager.OriginalFilename,
                       s.FileManager.GoogleImageId,
                       s.FileManager.Width,
                       s.FileManager.Height,
                       s.ImageHeight,
                       s.ImageWidth
                   };
        }
        public static IEnumerable GetContentLiquid(List<ContentLiquid> blogsLiquidList)
        {
            return from s in blogsLiquidList
                   select new
                   {
                       CategoryId = s.Content.CategoryId,
                       CategoryName = s.Category.Name,
                       CategoryDescription = s.Category.Description,
                       s.Content.Name,
                       s.Content.Description,
                       s.Content.Author,
                       s.Content.UpdatedDate,
                       s.DetailLink,
                       s.Content.VideoUrl,
                       s.PlainDescription,
                       images = s.ImageLiquid
                   };
        }

        public static object GetContentAnonymousObject(ContentLiquid contentLiquid)
        {
            object anonymousObject = new
            {
                CategoryId = contentLiquid.Content.CategoryId,
                CategoryName = contentLiquid.Category.Name,
                CategoryDescription = contentLiquid.Category.Description,
                ContentId = contentLiquid.Content.Id,
                Name = contentLiquid.Content.Name,
                Description = contentLiquid.Content.Description,
                contentLiquid.Content.VideoUrl,
                images = contentLiquid.ImageLiquid,

            };
            return anonymousObject;
        }

        public static object GetRetailer(RetailerLiquid retailerLiquid)
        {
            object anonymousObject = new
            {
                RetailerId = retailerLiquid.Retailer.Id,
                Name = retailerLiquid.Retailer.Name,
                retailerLiquid.Retailer.RetailerUrl
            };
            return anonymousObject;
        }
        public static object GetProductCategory(ProductCategoryLiquid productCategories)
        {
            object anonymousObject = new
            {
                CategoryId = productCategories.ProductCategory.Id,
                Name = productCategories.ProductCategory.Name,
                Description = productCategories.ProductCategory.Description
            };
            return anonymousObject;
        }
        public static object GetCategory(CategoryLiquid productCategories)
        {
            object anonymousObject = new
            {
                CategoryId = productCategories.Category.Id,
                Name = productCategories.Category.Name,
                Description = productCategories.Category.Description
            };
            return anonymousObject;
        }

        public static IEnumerable GetCategoriesLiquid(List<CategoryLiquid> cats)
        {
            return from s in cats
                   select new
                   {
                       s.Category.Name,
                       s.Category.Description,
                       s.DetailLink,
                       s.Count
                   };
        }
    }
}