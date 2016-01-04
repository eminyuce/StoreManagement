using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.Paging;

namespace  StoreManagement.Data.LiquidHelpers.Interfaces
{
    public interface IPhotoGalleryHelper : IHelper
    {
        StoreLiquidResult GetPhotoGalleryIndexPage(PageDesign pageDesign, List<FileManager> fileManagers);
        StoreLiquidResult GetPhotoGalleryIndexPage(PageDesign pageDesign, StorePagedList<FileManager> fileManagers);
    }

}