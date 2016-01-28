using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.IGeneralRepositories;

namespace StoreManagement.Service.ApiRepositories
{
    public class CommentApiRepository : BaseApiRepository, ICommentGeneralRepository
    {

        protected override string ApiControllerName { get { return "Comments"; } }


        public CommentApiRepository(string webServiceAddress) : base(webServiceAddress)
        {

        }



        protected override void SetCache()
        {
            HttpRequestHelper.CacheMinute = CacheMinute;
            HttpRequestHelper.IsCacheEnable = IsCacheEnable;
        }

        public Task<List<Comment>> GetCommentsByItemIdAsync(int storeId, int itemId, string itemType, int page, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
