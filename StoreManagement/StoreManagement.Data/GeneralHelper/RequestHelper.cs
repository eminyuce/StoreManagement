using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Newtonsoft.Json;
using RestSharp;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.Entities;
using StoreManagement.Data.HelpersModel;
using StoreManagement.Data.Paging;

namespace StoreManagement.Data.GeneralHelper
{
    public class RequestHelper
    {
 
        private static readonly TypedObjectCache<String> RequestHelperCache = new TypedObjectCache<String>("RequestHelperCache");
        private static CacheEntryUpdateCallback _callbackU = null;
        private bool _isCacheEnable = true;
        public bool IsCacheEnable
        {
            get { return _isCacheEnable; }
            set { _isCacheEnable = value; }
        }
        private int _cacheMinute = 30;
        public int CacheMinute
        {
            get { return _cacheMinute; }
            set { _cacheMinute = value; }
        }

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        readonly string _accountSid = "";
        readonly string _secretKey = "";

        public RequestHelper()
        {
            RequestHelperCache.IsCacheEnable = this.IsCacheEnable;
 
        }

        public RequestHelper(string accountSid, string secretKey)
            : base()
        {
            _accountSid = accountSid;
            _secretKey = secretKey;
        }
        public String GetJsonFromCacheOrWebservice(string url)
        {
            String key = url;
            String ret = "";
            Logger.Trace("key=" + url + " IsCacheEnable=" + IsCacheEnable);
            if (IsCacheEnable)
            {
                ret = (String)MemoryCache.Default.Get(key);
                if (String.IsNullOrEmpty(ret))
                {
                    ret = MakeJsonRequest(url);

                    CacheItemPolicy policy = null;
                    CacheEntryRemovedCallback callback = null;
                    policy = new CacheItemPolicy();
                    policy.Priority = CacheItemPriority.Default;
                    _callbackU = new CacheEntryUpdateCallback(ContentCacheUpdateCallback);
                    policy.UpdateCallback = _callbackU;
                    policy.AbsoluteExpiration = DateTime.Now.AddMinutes(_cacheMinute);

                    MemoryCache.Default.Set(key, ret, policy);

                }
                else
                {
                    Logger.Trace("Get data from Cache=" + url);
                }

            }
            else
            {
                ret = MakeJsonRequest(url);
                Logger.Trace("Get data from API=" + url);
            }
            return ret;

        }
        private void ContentCacheUpdateCallback(CacheEntryUpdateArguments arguments)
        {
            if (arguments.RemovedReason == CacheEntryRemovedReason.Expired)
            {
                var expiredCacheItem = MemoryCache.Default.GetCacheItem(arguments.Key);

                if (expiredCacheItem != null)
                {
                    String url = expiredCacheItem.Key;
                    Logger.Trace(String.Format("Return From ContentCacheUpdateCallback {0}", url));
                    var ret = GetJsonFromCacheOrWebservice(url);

                    expiredCacheItem.Value = ret;

                    arguments.UpdatedCacheItem = expiredCacheItem;

                    var policy = new CacheItemPolicy();
                    policy.Priority = CacheItemPriority.Default;

                    _callbackU = new CacheEntryUpdateCallback(ContentCacheUpdateCallback);
                    policy.UpdateCallback = _callbackU;
                    policy.AbsoluteExpiration = DateTime.Now.AddMinutes(_cacheMinute);


                    arguments.UpdatedCacheItemPolicy = policy;
                }
                else
                {
                    arguments.UpdatedCacheItem = null;
                }
            }

        }


        public Task<T> MakeJsonRequestAsync<T>(string url) where T : new()
        {
            try
            {
                var request = new RestRequest(Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-type", "application/json");
                var myClass = this.ExecuteAsync<T>(request, url);
                return myClass;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error:" + ex.StackTrace, url);
                return null;
            }

        }
        public Task<T> ExecuteAsync<T>(RestRequest request, String url) where T : new()
        {
            var client = new RestClient(url);
            var taskCompletionSource = new TaskCompletionSource<T>();
            // client.Authenticator = new HttpBasicAuthenticator(_accountSid, _secretKey);
            //request.AddParameter("AccountSid", _accountSid, ParameterType.UrlSegment);
            client.ExecuteAsync<T>(request, (response) => taskCompletionSource.SetResult(response.Data));
            return taskCompletionSource.Task;
        }
        public string MakeJsonRequest(string url)
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
                //var cacheControl = response.Headers.FirstOrDefault(r => r.Name.Equals("Cache-Control"));

            }
            else
            {
                returnJson = String.Empty;
            }
            return returnJson;
        }
        public static byte[] GetImageFromUrl(string url, Dictionary<String, String> dictionary)
        {
            System.Net.HttpWebRequest request = null;
            System.Net.HttpWebResponse response = null;
            byte[] b = null;

            if (dictionary == null)
            {
                dictionary = new Dictionary<String, String>();
            }

            request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            response = (System.Net.HttpWebResponse)request.GetResponse();

            if (request.HaveResponse)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    using (BinaryReader br = new BinaryReader(receiveStream))
                    {
                        b = br.ReadBytes(500000);
                        br.Close();
                    }

                    foreach (var h in response.Headers.AllKeys)
                    {
                        dictionary.Add(h, response.Headers[h]);
                    }
                    dictionary.Add("ContentType", response.ContentType);
                }
            }

            return b;
        }

       
        public IRestResponse PostBasicAuthenication(string baseUrl, string resourceUrl, string userName, string password, string json)
        {
            var client = new RestClient(baseUrl);
            client.Authenticator = new HttpBasicAuthenticator(userName, password);
            var request = new RestRequest(resourceUrl, Method.POST);
            request.AddParameter("text/json", json, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;
            return client.Execute(request);
        }


        public String GetPostJsonFromCacheOrWebservice(string baseUrl, string apiAddress, string json)
        {
            String url = String.Format("Post:{0}/{1}", baseUrl, apiAddress);
            String returnJson = String.Empty;

            if (RequestHelperCache.TryGet(url, out returnJson))
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
                    //    if (IsCacheEnable)
                    {

                        RequestHelperCache.Set(url, returnJson,
                                               MemoryCacheHelper.CacheAbsoluteExpirationPolicy(_cacheMinute));

                    }
                }
                Logger.Trace(String.Format("Return Categories From Webservise {0}", url));

            }

            return returnJson;
        }
        public IRestResponse MakeJsonPost(string baseUrl, string resourceUrl, string json)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(resourceUrl, Method.POST);
            request.AddParameter("text/json", json, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;
            return client.Execute(request);
        }



        public StorePagedList<T> GetUrlPagedResults<T>(string url) where T : new()
        {
            try
            {

                var responseContent = GetJsonFromCacheOrWebservice(url);
                if (!String.IsNullOrEmpty(responseContent))
                {
                    String jsonString = responseContent;
                   // var result = Jil.JSON.Deserialize<StorePagedList<T>>(jsonString, _jilOptions);
                    var result = JsonConvert.DeserializeObject<StorePagedList<T>>(jsonString);
                    //var jsonSer = new JsonSerializer<StorePagedList<T>>();
                    //var result = jsonSer.DeserializeFromString(jsonString);
                    return result;
                }
                else
                {
                    throw new Exception("Url:" + url + "  is not working");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error:" + ex.StackTrace, url);
                return new StorePagedList<T>();
            }
        }


        public List<T> GetUrlResults<T>(string url) where T : new()
        {
            try
            {
                //  

                var responseContent = GetJsonFromCacheOrWebservice(url);
                if (!String.IsNullOrEmpty(responseContent))
                {
                    String jsonString = responseContent;
                  //  var result = Jil.JSON.Deserialize<List<T>>(jsonString, _jilOptions);
                    var result = JsonConvert.DeserializeObject<List<T>>(jsonString);
                    //var jsonSer = new JsonSerializer<List<T>>();
                    //var result = jsonSer.DeserializeFromString(jsonString);
                    return result;
                }
                else
                {
                    throw new Exception("Url:" + url + "  is not working");
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error:" + ex.StackTrace, url);
                return new List<T>();
            }

        }

        public T GetUrlResult<T>(string url) where T : new()
        {
            try
            {
                //

                var responseContent = GetJsonFromCacheOrWebservice(url);
                if (!String.IsNullOrEmpty(responseContent))
                {
                    String jsonString = responseContent;
                    var result = JsonConvert.DeserializeObject<T>(jsonString);
                  //  var result = Jil.JSON.Deserialize<T>(jsonString);
                    //var jsonSer = new JsonSerializer<T>();
                    //var result = jsonSer.DeserializeFromString(jsonString);
                    return result;
                }

                throw new Exception("Url:" + url + "  is not working");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error:" + ex.StackTrace, url);
                return new T();
            }

        }
        public Task<StorePagedList<T>> GetUrlPagedResultsAsync<T>(string url) where T : new()
        {
            // return MakeJsonRequestAsync<StorePagedList<T>>(url);
            var task = new Task<StorePagedList<T>>(() => GetUrlPagedResults<T>(url));
            task.Start();

            return task;

        }
        public Task<T> GetUrlResultAsync<T>(string url) where T : new()
        {
            var task = new Task<T>(() => GetUrlResult<T>(url));
            task.Start();

            return task;

            // return MakeJsonRequestAsync<T>(url);
        }
        public Task<List<T>> GetUrlResultsAsync<T>(string url) where T : new()
        {
            var task = new Task<List<T>>(() => GetUrlResults<T>(url));
            task.Start();
            return task;

            //return MakeJsonRequestAsync<List<T>>(url);
        }
 
        public static Dictionary<String, String> UploadFileToWebStorage(String urlFilesPutfile, byte[] byteArray, string fileContentType, string fileName)
        {
            var resultImageDic = new Dictionary<String, String>();

            try
            {
                var client = new RestClient(string.Format(urlFilesPutfile));
                var request = new RestRequest(Method.POST);
                request.AddFile("file", byteArray, fileName, fileContentType);
                request.AddHeader("ContentType", fileContentType);
                request.AddHeader("FileName", fileName);

                var response = client.Execute(request);
                GetResponse(response, resultImageDic);
            }
            catch (Exception ex)
            {
                Logger.Error(String.Join("Exception is thrown while making request to storage fileName:{0} fileContentType:{1} Message:{2}", fileName, fileContentType, ex.Message), ex);
            }


            return resultImageDic;
        }

        private static void GetResponse(IRestResponse response, Dictionary<string, string> resultImageDic)
        {
            var statusCode = response.StatusCode;
            var p = response.Headers.FirstOrDefault(r => r.Name.Equals("FileNameUrl", StringComparison.InvariantCultureIgnoreCase));
            var p1 =
                response.Headers.FirstOrDefault(
                    r => r.Name.Equals("FileHash", StringComparison.InvariantCultureIgnoreCase));

            var isImageHeader = response.Headers.FirstOrDefault(r => r.Name.Equals("IsImage", StringComparison.InvariantCultureIgnoreCase));
            var isImage = isImageHeader != null && isImageHeader.Value.Equals("True");
            resultImageDic.Add("IsImage", isImage.ToStr());

            var heightHeader = response.Headers.FirstOrDefault(r => r.Name.Equals("Height", StringComparison.InvariantCultureIgnoreCase));
            if (heightHeader != null)
            {
                var height = heightHeader.Value.ToInt();
                resultImageDic.Add("Height", height.ToStr());
            }
            else
            {
                resultImageDic.Add("Height", "0");
            }

            var widthHeader = response.Headers.FirstOrDefault(r => r.Name.Equals("Width", StringComparison.InvariantCultureIgnoreCase));
            if (widthHeader != null)
            {
                var width = widthHeader.Value.ToInt();
                resultImageDic.Add("Width", width.ToStr());
            }
            else
            {
                resultImageDic.Add("Width", "0");
            }


            if (p != null)
            {
                var fileNameUrl = p.Value.ToStr();
                resultImageDic.Add("FileNameUrl", fileNameUrl);
            }
            else
            {
                resultImageDic.Add("FileNameUrl", "");
            }

            if (p1 != null)
            {
                var fileHash = p1.Value.ToStr();
                resultImageDic.Add("FileHash", fileHash);
            }
            else
            {
                resultImageDic.Add("FileHash", "");
            }
        }
    }


}
