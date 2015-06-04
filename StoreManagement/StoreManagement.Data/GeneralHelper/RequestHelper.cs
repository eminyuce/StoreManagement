using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using MvcPaging;
using NLog;
using RestSharp;
using ServiceStack.Text;
using StoreManagement.Data.HelpersModel;
using StoreManagement.Data.Paging;

namespace StoreManagement.Data.GeneralHelper
{
    public class RequestHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static CacheEntryUpdateCallback _callbackU = null;
        private static int _cacheSecond = 600;
        public static int CacheSecond
        {
            get { return _cacheSecond; }
            set { _cacheSecond = value; }
        }
        public  String GetJsonFromCacheOrWebservice(string url)
        {

            String returnJson = String.Empty;
            returnJson = CacheResponseOutput(url, String.Empty);
            if (!String.IsNullOrEmpty(returnJson))
            {
                Logger.Info(String.Format("Return Categories From Cache {0}", url));
                return returnJson;
            }
            else
            {
                String responseJson = MakeJsonRequest(url);
                returnJson = CacheResponseOutput(url, responseJson);
                Logger.Info(String.Format("Return Categories From Webservise {0}", url));
            }

            return returnJson;
        }

        public  string MakeJsonRequest(string url)
        {
            string returnJson = String.Empty;
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-type", "application/json");
            var response = client.Execute(request);
            HttpStatusCode statusCode = response.StatusCode;
            if (statusCode == HttpStatusCode.OK)
            {
                returnJson = response.Content;
                var cacheControl = response.Headers.FirstOrDefault(r => r.Name.Equals("Cache-Control"));
                if (cacheControl != null)
                {
                    String cacheValue = cacheControl.Value.ToStr();
                    if (cacheValue.StartsWith("public"))
                    {
                        CacheSecond = cacheValue.Substring(cacheValue.IndexOf("=") + 1).ToInt();
                    }
                }
            }
            else
            {
                returnJson = String.Empty;
            }
            return returnJson;
        }
        private  string CacheResponseOutput(string key, String responseContent)
        {
            var ret = (String)MemoryCache.Default.Get(key);
            if (String.IsNullOrEmpty(ret))
            {
                ret = responseContent;
                CacheItemPolicy policy = null;
                CacheEntryRemovedCallback callback = null;
                policy = new CacheItemPolicy();
                policy.Priority = CacheItemPriority.Default;
                _callbackU = new CacheEntryUpdateCallback(ContentCacheUpdateCallback);
                policy.UpdateCallback = _callbackU;
                policy.AbsoluteExpiration = DateTime.Now.AddSeconds(CacheSecond);
                MemoryCache.Default.Set(key, ret, policy);
            }
            return ret;
        }

        private  void ContentCacheUpdateCallback(CacheEntryUpdateArguments arguments)
        {
            if (arguments.RemovedReason == CacheEntryRemovedReason.Expired)
            {
                var expiredCacheItem = MemoryCache.Default.GetCacheItem(arguments.Key);

                if (expiredCacheItem != null)
                {
                    String url = expiredCacheItem.Key;
                    Logger.Info(String.Format("Return From ContentCacheUpdateCallback {0}", url));
                    var ret = GetJsonFromCacheOrWebservice(url);

                    expiredCacheItem.Value = ret;

                    arguments.UpdatedCacheItem = expiredCacheItem;

                    var policy = new CacheItemPolicy();
                    policy.Priority = CacheItemPriority.Default;

                    _callbackU = new CacheEntryUpdateCallback(ContentCacheUpdateCallback);
                    policy.UpdateCallback = _callbackU;
                    policy.AbsoluteExpiration = DateTime.Now.AddSeconds(CacheSecond);


                    arguments.UpdatedCacheItemPolicy = policy;
                }
                else
                {
                    arguments.UpdatedCacheItem = null;
                }
            }

        }

        public  IRestResponse PostBasicAuthenication(string baseUrl, string resourceUrl, string userName, string password, string json)
        {
            var client = new RestClient(baseUrl);
            client.Authenticator = new HttpBasicAuthenticator(userName, password);
            var request = new RestRequest(resourceUrl, Method.POST);
            request.AddParameter("text/json", json, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;
            return client.Execute(request);
        }


        public  String GetPostJsonFromCacheOrWebservice(string baseUrl, string apiAddress, string json)
        {
            String url = String.Format("Post:{0}/{1}", baseUrl, apiAddress);
            String returnJson = String.Empty;
            returnJson = CacheResponseOutput(url, String.Empty);
            if (!String.IsNullOrEmpty(returnJson))
            {
                Logger.Info(String.Format("Return Categories From Cache {0}", url));
                return returnJson;
            }
            else
            {
                var response = MakeJsonPost(baseUrl, apiAddress, json);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    returnJson = response.Content;
                    CacheResponseOutput(url, response.Content);
                }
                Logger.Info(String.Format("Return Categories From Webservise {0}", url));

            }

            return returnJson;
        }
        public  IRestResponse MakeJsonPost(string baseUrl, string resourceUrl, string json)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(resourceUrl, Method.POST);
            request.AddParameter("text/json", json, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;
            return client.Execute(request);
        }

        public  string ConvertObjectToJason<T>(T arg)
        {
           // return JsonConvert.SerializeObject(arg);

            var jsonSer = new JsonSerializer<T>();
            var result = jsonSer.SerializeToString(arg);
            return result;


        }
        public StorePagedList<T>  GetUrlPagedResults<T>(string url) where T : new()
        {
            var responseContent = GetJsonFromCacheOrWebservice(url);
            if (!String.IsNullOrEmpty(responseContent))
            {
                String jsonString = responseContent;
                //var result = JsonConvert.DeserializeObject<StorePagedList<T>>(jsonString);
                var jsonSer = new JsonSerializer<StorePagedList<T>>();
                var result = jsonSer.DeserializeFromString(jsonString);
                return result;
            }
            else
            {
                throw new Exception("Url:" + url + "  is not working");
            }
        }
        public  List<T> GetUrlResults<T>(string url)
        {
            var responseContent = GetJsonFromCacheOrWebservice(url);
            if (!String.IsNullOrEmpty(responseContent))
            {
                String jsonString = responseContent;
                var jsonSer = new JsonSerializer<List<T>>();
                var result = jsonSer.DeserializeFromString(jsonString);
                return result;
            }
            else
            {
                throw new Exception("Url:" + url + "  is not working");
            }
        }
        public  T GetUrlResult<T>(string url) where T : new()
        {
            var responseContent = GetJsonFromCacheOrWebservice(url);
            if (!String.IsNullOrEmpty(responseContent))
            {
                String jsonString = responseContent;
                var jsonSer = new JsonSerializer<T>();
                var result = jsonSer.DeserializeFromString(jsonString);
                return result;
            }

            throw new Exception("Url:" + url + "  is not working");
        }
    }


}
