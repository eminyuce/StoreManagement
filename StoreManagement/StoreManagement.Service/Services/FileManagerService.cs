using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcPaging;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.Entities;
using StoreManagement.Data.Paging;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.Services.IServices;

namespace StoreManagement.Service.Services
{
    public class FileManagerService : BaseService, IFileManagerService
    {
        private static readonly TypedObjectCache<List<FileManager>> StoreFileManagerCache
= new TypedObjectCache<List<FileManager>>("StoreFileManager");

        private static readonly TypedObjectCache<StorePagedList<FileManager>> StorePagedListFileManagerCache
 = new TypedObjectCache<StorePagedList<FileManager>>("StorePagedListFileManagerCache");


        public PhotosViewModel GetPhotos(int page)
        {
            var resultModel = new PhotosViewModel();
            resultModel.SStore = this.MyStore;
            var m = FileManagerRepository.GetImagesByStoreId(MyStore.Id, page, 24);
            resultModel.SFileManagers = new PagedList<FileManager>(m.items, m.page - 1, m.pageSize, m.totalItemCount);
            resultModel.SNavigations = NavigationRepository.GetStoreActiveNavigations(this.MyStore.Id);
            resultModel.SSettings = this.GetStoreSettings();
            return resultModel;
        }

        public List<FileManager> GetFilesByStoreId(int storeId)
        {
            return FileManagerRepository.GetFilesByStoreId(storeId);
        }
    }
}
