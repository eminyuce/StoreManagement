using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Services
{
    public class LabelService : BaseService, ILabelService
    {

        private const String ApiControllerName = "Labels";

        public LabelService(string webServiceAddress)
            : base(webServiceAddress)
        {

        }

        public List<Label> GetLabelsByLabelType(string labelType)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetLabelsByLabelType?labelType={2}", WebServiceAddress, ApiControllerName, labelType);
            return HttpRequestHelper.GetUrlResults<Label>(url);
        }

        public List<Label> GetLabelsByLabelType(int storeId, string labelType)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetLabelsByLabelType?storeId={2}&labelType={3}", WebServiceAddress, ApiControllerName, storeId, labelType);
            return HttpRequestHelper.GetUrlResults<Label>(url);
        }

        public List<Label> GetLabelsByTypeAndCategoryAndSearch(int storeId, string labelType, int categoryId, string search)
        {
            throw new NotImplementedException();
        }

        protected override void SetCache()
        {
            HttpRequestHelper.CacheMinute = CacheMinute;
            HttpRequestHelper.IsCacheEnable = IsCacheEnable;
        }
    }
}
