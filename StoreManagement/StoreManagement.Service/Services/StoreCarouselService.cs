using StoreManagement.Data.GeneralHelper;
using StoreManagement.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.Services
{
    public class StoreCarouselService : BaseService, IStoreCarouselService
    {
        private const String ApiControllerName = "StoreCarousels";

        public StoreCarouselService(string webServiceAddress) : base(webServiceAddress)
        {

        }

        public List<StoreCarousel> GetStoreCarousels(int storeId)
        {
            try
            {
                string url = string.Format("http://{0}/api/{1}/GetStoreCarousels?storeId={2}", WebServiceAddress, ApiControllerName, storeId);
                return RequestHelper.GetUrlResults<StoreCarousel>(url);
            }
            catch (Exception ex)
            {
                Logger.ErrorException("Error:" + ex.Message, ex);
                return new List<StoreCarousel>();
            }
        }
    }
}
