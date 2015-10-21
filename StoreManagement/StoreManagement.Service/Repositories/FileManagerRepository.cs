using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;
using GenericRepository.EntityFramework.Enums;
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

        public async Task<List<FileManager>> GetImagesByStoreIdAsync(int storeId, bool? isActive)
        {

            try
            {
                Expression<Func<FileManager, bool>> match = r2 => r2.StoreId == storeId && r2.State;
                Expression<Func<FileManager, int>> keySelector = t => t.Ordering;
                var items = this.FindAllIncludingAsync(match, null, null, keySelector, OrderByType.Descending);
                return await items;
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }
        }

        public List<FileManager> GetImagesByStoreId(int storeId, bool? isActive)
        {
            try
            {
                Expression<Func<FileManager, bool>> match = r2 => r2.StoreId == storeId && r2.State;
                var items = this.FindBy(match).ToList();
                return items;
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }
        }

        public async Task<List<FileManager>> GetStoreCarouselsAsync(int storeId, int? take)
        {
            try
            {
                Expression<Func<FileManager, bool>> match = r2 => r2.StoreId == storeId && r2.State && r2.IsCarousel;
                Expression<Func<FileManager, int>> keySelector = t => t.Ordering;
                var items = this.FindAllIncludingAsync(match, take, null, keySelector, OrderByType.Descending);
                return await items;
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }
        }

        public List<FileManager> GetFilesByStoreIdAndLabels(int storeId, string[] labels)
        {
            var labelIds = labels.Select(r => r.ToInt());


            var res = from s in this.StoreDbContext.FileManagers
                      join c in this.StoreDbContext.LabelLines on s.Id equals c.ItemId
                      join u in this.StoreDbContext.Labels on c.LabelId equals u.Id
                      where s.StoreId == storeId && u.StoreId == storeId && c.ItemType.Equals(StoreConstants.Files) && labelIds.Contains(u.Id)
                      orderby s.Ordering descending 
                      select s;

            var result = res.Distinct().ToList();

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
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }



}
