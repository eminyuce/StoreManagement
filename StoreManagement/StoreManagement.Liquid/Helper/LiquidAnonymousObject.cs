using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                       s.ImageSource
                   };
        }
        public static IEnumerable GetActivitiesEnumerable(List<ActivitiesLiquid> items)
        {
            return from s in items
                   select new
                   {
                       s.Activity.Name

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
            return from s in items
                   select new
                   {
                       s.Contact.Name,
                       s.Contact.Title,
                       s.Contact.PhoneCell,
                       s.Contact.PhoneWork,
                       s.Contact.Email
                   };
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
                       s.Link
                   };
        }
        public static IEnumerable GetProductsLiquid(List<ProductLiquid> productLiquidList)
        {
            return from s in productLiquidList
                   select new
                   {
                       CategoryName = s.Category.Name,
                       ProductCategoryId = s.Product.ProductCategoryId,
                       BrandId = s.Product.BrandId,
                       CategoryDescription = s.Category.Description,
                       ProductId = s.Product.Id,
                       s.Product.Name,
                       s.Product.Description,
                       s.Product.ProductCode,
                       s.Product.Price,
                       s.Product.Discount,
                       s.Product.UpdatedDate,
                       s.Product.CreatedDate,
                       s.Product.TotalRating,
                       s.Product.UnitsInStock,
                       s.DetailLink,
                       images = s.ImageLiquid

                   };
        }
        public static object GetProductAnonymousObject(ProductLiquid s)
        {
            object anonymousObject = new
            {
                CategoryName = s.Category.Name,
                ProductCategoryId = s.Product.ProductCategoryId,
                BrandId = s.Product.BrandId,
                CategoryDescription = s.Category.Description,
                ProductId = s.Product.Id,
                s.Product.Name,
                s.Product.Description,
                s.Product.ProductCode,
                s.Product.Price,
                s.Product.Discount,
                s.Product.UpdatedDate,
                s.Product.CreatedDate,
                s.Product.TotalRating,
                s.Product.UnitsInStock,
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
                       s.Content.Name,
                       s.Content.Description,
                       s.Content.Author,
                       s.Content.UpdatedDate,
                       s.DetailLink,
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
                images = contentLiquid.ImageLiquid
            };
            return anonymousObject;
        }
    }
}