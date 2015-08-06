using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;
using MvcPaging;
using StoreManagement.Data;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.Paging;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Service.Repositories
{
    public class FileManagerRepository : BaseRepository<FileManager, int>, IFileManagerRepository
    {

        private static readonly TypedObjectCache<List<FileManager>> StoreFileManagerCache
      = new TypedObjectCache<List<FileManager>>("StoreFileManager");

        private static readonly TypedObjectCache<StorePagedList<FileManager>> StorePagedListFileManagerCache
 = new TypedObjectCache<StorePagedList<FileManager>>("StorePagedListFileManagerCache");


        public FileManagerRepository(IStoreContext dbContext)
            : base(dbContext)
        {
        }

        public List<FileManager> GetFilesByStoreIdFromCache(int storeId)
        {
            String key = String.Format("StoreFileManager-{0}", storeId);
            List<FileManager> items = null;
            StoreFileManagerCache.TryGet(key, out items);
            if (items == null)
            {
                items = GetFilesByStoreId(storeId);
                StoreFileManagerCache.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("FileManager_CacheAbsoluteExpiration_Minute", 10)));
            }
            return items;
        }

        public List<FileManager> GetFilesByStoreId(int storeId)
        {
            return FindBy(r => r.StoreId == storeId).ToList();
        }

        public FileManager GetFilesByGoogleImageId(string googleImageId)
        {
            return
                FindBy(r => r.GoogleImageId.Equals(googleImageId, StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault();
        }

        public List<FileManager> GetFilesByGoogleImageIdArray(string[] googleImageId)
        {
            return
                FindBy(r => googleImageId.Contains(r.GoogleImageId)).ToList();
        }

        public FileManager GetFilesById(int id)
        {
            return GetSingle(id);
        }

        public List<FileManager> GetStoreCarousels(int storeId)
        {
            return FindBy(r => r.StoreId == storeId).Where(r => r.IsCarousel).ToList();
        }

        public StorePagedList<FileManager> GetImagesByStoreId(int storeId, int page, int pageSize)
        {
            String key = String.Format("StoreFileManager-{0}", storeId);
            StorePagedList<FileManager> items = null;
            StorePagedListFileManagerCache.TryGet(key, out items);
            if (items == null)
            {
                var images = FindBy(r => r.StoreId == storeId).ToList();
                items = new StorePagedList<FileManager>(images.Skip((page - 1) * pageSize).Take(pageSize).ToList(), page, pageSize, images.Count());
                StorePagedListFileManagerCache.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("FileManager_CacheAbsoluteExpiration_Minute", 10)));
            }
            return items;
        }

        public Task<List<FileManager>> GetImagesByStoreIdAsync(int storeId, bool? isActive)
        {
            var res = Task.FromResult(GetFilesByStoreId(storeId).Where(r => r.State == isActive).ToList());
            return res;
        }

        public Task<List<FileManager>> GetStoreCarouselsAsync(int storeId, int? take)
        {
            var task = Task.Factory.StartNew(() =>
                 {
                     var items = GetFilesByStoreId(storeId).Where(r => r.IsCarousel);
                     if (take.HasValue)
                     {
                         items = items.Take(take.Value);
                     }

                     return items.OrderBy(r => r.Ordering).ToList();

                 });
            return task;
        }

        public List<FileManager> GetFilesByStoreIdAndLabels(int storeId, string[] labels)
        {
            var labelIds = labels.Select(r => r.ToInt());
            var res = from s in this.StoreDbContext.FileManagers
                      join c in this.StoreDbContext.LabelLines on s.Id equals c.ItemId
                      join u in this.StoreDbContext.Labels on c.LabelId equals u.Id
                      where s.StoreId == storeId && u.StoreId == storeId && c.ItemType.Equals(StoreConstants.Files) && labelIds.Contains(u.Id)
                      select s;

            var result = res.ToList();

            return result;
        }

        public List<FileManager> GetFilesBySearchKey(int storeId, string search)
        {
            var items = FindBy(r => r.StoreId == storeId);

            if (!String.IsNullOrEmpty(search))
            {
                items = items.Where(r => r.OriginalFilename.ToLower().Contains(search.ToLower()));
            }

            return items.OrderBy(r => r.Ordering).ToList();
        }

        public List<FileManager> GetFilesByFileStatus(string fileStatus)
        {
            var items = FindBy(r => r.FileStatus.Equals(fileStatus, StringComparison.InvariantCultureIgnoreCase)).ToList();

            return items;
        }
    }



}
