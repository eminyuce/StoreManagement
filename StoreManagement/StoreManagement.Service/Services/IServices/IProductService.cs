using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.ActionResults;
using StoreManagement.Data.RequestModel;

namespace StoreManagement.Service.Services.IServices
{
    public interface IProductService : IBaseService
    {

        ProductDetailViewModel GetProductDetailPage(string id);

        ProductsViewModel GetProductIndexPage(string search, string page);

        StoreHomePage GetHomePage();

        FeedResult GetProductRss(int take, int description, int imageHeight, int imageWidth, int isDetailLink);
    }
}
