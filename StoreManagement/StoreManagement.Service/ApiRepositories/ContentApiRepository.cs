using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.HelpersModel;
using StoreManagement.Data.Paging;
using StoreManagement.Service.IGeneralRepositories;

namespace StoreManagement.Service.ApiRepositories
{
    public class ContentApiRepository : BaseApiRepository, IContentGeneralRepository
    {

        protected override string ApiControllerName { get { return "Contents"; } }

        public ContentApiRepository(string webServiceAddress)
            : base(webServiceAddress)
        {

        }

        public Content GetContentsContentId(int contentId)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetContentsContentId?contentId={2}", WebServiceAddress, ApiControllerName, contentId);
            return HttpRequestHelper.GetUrlResult<Content>(url);

        }

        public List<Content> GetContentByType(string typeName)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetContentByType?typeName={2}", WebServiceAddress, ApiControllerName, typeName);
            return HttpRequestHelper.GetUrlResults<Content>(url);

        }

        public List<Content> GetContentByType(int storeId, string typeName)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetContentByType?storeId={2}&typeName={3}", WebServiceAddress, ApiControllerName, storeId, typeName);
            return HttpRequestHelper.GetUrlResults<Content>(url);

        }

        public Content GetContentByUrl(int storeId, string url)
        {
            SetCache();
            string url2 = string.Format("http://{0}/api/{1}/GetContentByUrl?storeId={2}&url={3}", WebServiceAddress, ApiControllerName, storeId, url);
            return HttpRequestHelper.GetUrlResult<Content>(url2);

        }

       

        public List<Content> GetContentByTypeAndCategoryId(int storeId, string typeName, int categoryId, string search, bool? isActive)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetContentByTypeAndCategoryId?" +
                                        "storeId={2}" +
                                        "&typeName={3}&categoryId={4}&search={5}&isActive={6}",
                                        WebServiceAddress,
                                        ApiControllerName,
                                        storeId,
                                        typeName,
                                        categoryId, search, isActive);

            return HttpRequestHelper.GetUrlResults<Content>(url);
        }

        public List<Content> GetContentByTypeAndCategoryIdFromCache(int storeId, string typeName, int categoryId)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetContentByTypeAndCategoryIdFromCache" +
                                       "?storeId={2}" +
                                       "&typeName={3}" +
                                       "&categoryId={4}", WebServiceAddress, ApiControllerName, storeId, typeName, categoryId);
            return HttpRequestHelper.GetUrlResults<Content>(url);

        }

        public StorePagedList<Content> GetContentsCategoryId(int storeId, int? categoryId, string typeName, bool? isActive, int page, int pageSize)
        {
            SetCache();

            string url = string.Format("http://{0}/api/{1}/GetContentsCategoryId?storeId={2}" +
                                       "&categoryId={3}" +
                                       "&typeName={4}" +
                                       "&isActive={5}&page={6}&pageSize={7}", WebServiceAddress, ApiControllerName, storeId, categoryId, typeName, isActive, page, pageSize);
            return HttpRequestHelper.GetUrlPagedResults<Content>(url);

        }

        public Content GetContentWithFiles(int id)
        {

            string url = string.Format("http://{0}/api/{1}/GetContentWithFiles?id={2}", WebServiceAddress, ApiControllerName, id);

            return HttpRequestHelper.GetUrlResult<Content>(url);

        }


        public Task<StorePagedList<Content>> GetContentsCategoryIdAsync(int storeId, int? categoryId, string typeName, bool? isActive, int page, int pageSize,
                                               string search)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetContentsCategoryIdAsync?storeId={2}" +
                                       "&categoryId={3}" +
                                       "&typeName={4}" +
                                       "&isActive={5}&page={6}&pageSize={7}&search={8}",
                                       WebServiceAddress, ApiControllerName, storeId, categoryId, typeName, isActive, page, pageSize, search);
            return HttpRequestHelper.GetUrlPagedResultsAsync<Content>(url);
        }

        public Task<Content> GetContentByIdAsync(int id)
        {
            string url = string.Format("http://{0}/api/{1}/GetContentByIdAsync?id={2}", WebServiceAddress, ApiControllerName, id);
            return HttpRequestHelper.GetUrlResultAsync<Content>(url);
        }

        public Task<List<Content>> GetContentByTypeAndCategoryIdAsync(int storeId, string typeName, int categoryId, int take)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetContentByTypeAndCategoryIdAsync?storeId={2}&typeName={3}&categoryId={4}&take={5}", WebServiceAddress, ApiControllerName, storeId, typeName, categoryId, take);
            return HttpRequestHelper.GetUrlResultsAsync<Content>(url);
        }

        public Task<List<Content>> GetContentByTypeAndCategoryIdAsync(int storeId, string typeName, int categoryId, int take, int? excludedContentId)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetContentByTypeAndCategoryIdAsync?storeId={2}&typeName={3}&categoryId={4}&take={5}&excludedContentId={6}", WebServiceAddress, ApiControllerName, storeId, typeName, categoryId, take, excludedContentId);
            return HttpRequestHelper.GetUrlResultsAsync<Content>(url);
        }

        public Task<List<Content>> GetMainPageContentsAsync(int storeId, int? categoryId, string type, int? take)
        {
            SetCache();
            string url = string.Format("http://{0}/api/{1}/GetMainPageContentsAsync?storeId={2}&categoryId={3}&type={4}&take={5}", WebServiceAddress, ApiControllerName, storeId, categoryId, type, take);
            return HttpRequestHelper.GetUrlResultsAsync<Content>(url);
        }

        public Task<List<Content>> GetContentByTypeAsync(int storeId, int? take, bool? isActive, string typeName)
        {
            try
            {
                SetCache();
                string url = string.Format("http://{0}/api/{1}/GetContentByTypeAsync?storeId={2}&take={3}&isActive={4}&typeName={5}", WebServiceAddress, ApiControllerName, storeId, take, isActive, typeName);
                return HttpRequestHelper.GetUrlResultsAsync<Content>(url);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public List<Content> GetContentByType(int storeId, int? take, bool? isActive, string typeName)
        {
            try
            {
                SetCache();
                string url = string.Format("http://{0}/api/{1}/GetContentByType?storeId={2}&take={3}&isActive={4}&typeName={5}", WebServiceAddress, ApiControllerName, storeId, take, isActive, typeName);
                return HttpRequestHelper.GetUrlResults<Content>(url);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        public Task<List<Content>> GetContentsByContentKeywordAsync(int storeId, int? catId, string type, int page, int pageSize, bool? isActive,
                                                     string contentType)
        {
            try
            {
                SetCache();
                string url = string.Format("http://{0}/api/{1}/GetContentsByContentKeywordAsync?" +
                                                 "storeId={2}&catId={3}&type={4}" +
                                                 "&page={5}&pageSize={6}&isActive={7}&contentType={8}",
                                                 WebServiceAddress,
                                                 ApiControllerName,
                                                 storeId, catId, type,
                                                 page, pageSize, isActive, contentType);

                return HttpRequestHelper.GetUrlResultsAsync<Content>(url);


            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                return null;
            }
        }


        protected override void SetCache()
        {
            HttpRequestHelper.CacheMinute = CacheMinute;
            HttpRequestHelper.IsCacheEnable = IsCacheEnable;
        }
    }
}
