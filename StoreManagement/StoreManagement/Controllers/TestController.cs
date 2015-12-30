using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using StoreManagement.Data.Constants;
using StoreManagement.Data.LiquidEntities;

namespace StoreManagement.Controllers
{
    public class TestController : BaseController
    {
        //
        // GET: /Test/
        public async Task<ActionResult> Index()
        {

            var productsTask = ProductService.GetProductsAsync(this.StoreId, 10, true);
            var categoriesTask = ProductCategoryService.GetProductCategoriesByStoreIdAsync(StoreId, StoreConstants.ProductType, true);
            await Task.WhenAll(productsTask, categoriesTask);
            var productsObk = productsTask.Result;
            var categories = categoriesTask.Result;
            var products = new List<ProductLiquid>();
            foreach (var item in productsObk)
            {
                var category = categories.FirstOrDefault(r => r.Id == item.ProductCategoryId);
                if (category != null)
                {
                    var blog = new ProductLiquid(item, category, 10, 10);
                    products.Add(blog);
                }
            }

            return View(products);
        }
	}
}