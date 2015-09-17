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
using StoreManagement.Liquid.Helper.Interfaces;

namespace StoreManagement.Liquid.Helper
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

                    var blog = new BrandLiquid(item, pageDesign, ImageWidth, ImageHeight);
                    items.Add(blog);

                }


                object anonymousObject = new
                    {
                        brands = LiquidAnonymousObject.GetBrandsEnumerable(items)


                    };

                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);


                dic[StoreConstants.PageOutput] = indexPageOutput;

            }
            catch (Exception ex)
            {
                Logger.Error(ex);

            }


            var result = new StoreLiquidResult();
            result.LiquidRenderedResult = dic;
            return result;

        }

      


        public StoreLiquidResult GetBrandDetailPage(Brand brand, List<Product> products, PageDesign pageDesign, List<ProductCategory> productCategories)
        {
            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, "");
            try
            {
             


                var brandLiquid = new BrandLiquid(brand, pageDesign, ImageWidth, ImageHeight);
                brandLiquid.Products = products;
                brandLiquid.ProductCategories = productCategories;

                object anonymousObject = new
                {
                    brand=brandLiquid,
                    name=brandLiquid.Brand.Name,
                    description = brandLiquid.Brand.Description,
                    products = LiquidAnonymousObject.GetProductsLiquid(brandLiquid.ProductLiquidList),
                    productcategories = LiquidAnonymousObject.GetProductCategories(brandLiquid.ProductCategoriesLiquids)

                };
                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);


                dic[StoreConstants.PageOutput] = indexPageOutput;

            }
            catch (Exception ex)
            {
                Logger.Error(ex);

            }


            var result = new StoreLiquidResult();
            result.LiquidRenderedResult = dic;
            return result;
        }
    }
}