using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEngineHelpers;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Liquid.Helper.Interfaces;

namespace StoreManagement.Liquid.Helper
{
    public class RetailerHelper : BaseLiquidHelper, IRetailerHelper
    {
        public StoreLiquidResult GetRetailers(List<Retailer> labels,
                                       PageDesign pageDesign)
        {


            var items = new List<RetailerLiquid>();
            foreach (var item in labels)
            {

                var nav = new RetailerLiquid(item);
                items.Add(nav);
            }


            object anonymousObject = new
            {
                labels = LiquidAnonymousObject.GetRetailersEnumerable(items)


            };


            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);


            var dic = new Dictionary<String, String>();

            dic.Add(StoreConstants.PageOutput, indexPageOutput);


            var result = new StoreLiquidResult();
            result.PageDesingName = pageDesign.Name;
            result.LiquidRenderedResult = dic;
            return result;
        }


        public StoreLiquidResult GetRetailerDetailPage(Retailer retailer, List<Product> products, PageDesign pageDesign, List<ProductCategory> productCategories)
        {
            var result = new StoreLiquidResult();
            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, "");

            try
            {

                var retailerLiquid = new RetailerLiquid(retailer, ImageWidth, ImageHeight);
                retailerLiquid.Products = products;
                retailerLiquid.ProductCategories = productCategories;

                object anonymousObject = new
                {
                    retailer = retailerLiquid,
                    name = retailerLiquid.Retailer.Name,
                    retailerLiquid.Retailer.RetailerUrl,
                    products = LiquidAnonymousObject.GetProductsLiquid(retailerLiquid.ProductLiquidList),
                    productcategories = LiquidAnonymousObject.GetProductCategories(retailerLiquid.ProductCategoriesLiquids)

                };
                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);


                dic[StoreConstants.PageOutput] = indexPageOutput;
                result.DetailLink = retailerLiquid.DetailLink;

            }
            catch (Exception ex)
            {
                Logger.Error(ex);

            }



            result.LiquidRenderedResult = dic;
            result.PageDesingName = pageDesign.Name;

            return result;
        }
    }
}