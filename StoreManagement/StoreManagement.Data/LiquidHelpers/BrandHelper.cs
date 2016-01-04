using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.LiquidEngineHelpers;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.Paging;
using StoreManagement.Data.LiquidHelpers.Interfaces;

namespace StoreManagement.Data.LiquidHelpers
{

    public class BrandHelper : BaseLiquidHelper, IBrandHelper
    {


        public StoreLiquidResult GetBrandsPartial(List<Brand> brands, PageDesign pageDesign)
        {
            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, "");
            try
            {


                var items = new List<BrandLiquid>();
                foreach (var item in brands)
                {

                    var blog = new BrandLiquid(item, ImageWidth, ImageHeight);
                    items.Add(blog);

                }


                object anonymousObject = new
                    {
                        brands = LiquidAnonymousObject.GetBrandsEnumerable(items)


                    };

                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign, anonymousObject);


                dic[StoreConstants.PageOutput] = indexPageOutput;

            }
            catch (Exception ex)
            {
                Logger.Error(ex);

            }


            var result = new StoreLiquidResult();
            result.LiquidRenderedResult = dic;
            result.PageDesingName = pageDesign.Name;
            return result;

        }




        public StoreLiquidResult GetBrandDetailPage(Brand brand, List<Product> products, PageDesign pageDesign, List<ProductCategory> productCategories)
        {
            var result = new StoreLiquidResult();
            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, "");
         
            try
            {

                var brandLiquid = new BrandLiquid(brand, ImageWidth, ImageHeight);
                brandLiquid.Products = products;
                brandLiquid.ProductCategories = productCategories;

                object anonymousObject = new
                {
                    brand = LiquidAnonymousObject.GetBrandLiquid(brandLiquid),
                    products = LiquidAnonymousObject.GetProductsLiquid(brandLiquid.ProductLiquidList),
                    productCategories = LiquidAnonymousObject.GetProductCategories(brandLiquid.ProductCategoriesLiquids)

                };
                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign, anonymousObject);


                dic[StoreConstants.PageOutput] = indexPageOutput;
                result.DetailLink = brandLiquid.DetailLink;

            }
            catch (Exception ex)
            {
                Logger.Error(ex);

            }



            result.LiquidRenderedResult = dic;
            result.PageDesingName = pageDesign.Name;

            return result;
        }

        public StoreLiquidResult GetBrandsIndexPage(PageDesign pageDesign, StorePagedList<Brand> brands)
        {
            var result = new StoreLiquidResult();

            try
            {
                var brandList = new List<BrandLiquid>();
                foreach (var item in brands.items)
                {
                    brandList.Add(new BrandLiquid(item, this.ImageWidth, this.ImageHeight));
                }

                object anonymousObject = new
                {
                    brands = LiquidAnonymousObject.GetBrandsEnumerable(brandList)
                };

                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign, anonymousObject);


                var dic = new Dictionary<String, String>();
                dic.Add(StoreConstants.PageOutput, indexPageOutput);
                dic.Add(StoreConstants.PageSize, brands.pageSize.ToStr());
                dic.Add(StoreConstants.PageNumber, brands.page.ToStr());
                dic.Add(StoreConstants.TotalItemCount, brands.totalItemCount.ToStr());

                result.LiquidRenderedResult = dic;
                result.PageDesingName = pageDesign.Name;

            }
            catch (Exception exception)
            {
                Logger.Error(exception, "GetbrandsIndexPage : brands and pageDesign", String.Format("brands Items Count : {0}", brands.items.Count));
            }

            return result;

        }
    }
}