using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.IGeneralRepositories;

namespace StoreManagement.Service.ApiRepositories
{
    public class LabelApiRepository : BaseApiRepository, ILabelGeneralRepository
    {

        protected override string ApiControllerName { get { return "Labels"; } }
        public LabelApiRepository(string webServiceAddress)
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



        protected override void SetCache()
        {
            HttpRequestHelper.CacheMinute = CacheMinute;
            HttpRequestHelper.IsCacheEnable = IsCacheEnable;
        }

        public Task<List<Label>> GetLabelsByItemTypeId(int storeId, int itemId, string itemType)
        {
            try
            {
                SetCache();
                string url = string.Format("http://{0}/api/{1}/GetLabelsByItemTypeId?storeId={2}&itemId={3}&itemType={4}", WebServiceAddress, ApiControllerName, storeId, itemId, itemType);
                return HttpRequestHelper.GetUrlResultsAsync<Label>(url);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;

            }
        }
    }
}
