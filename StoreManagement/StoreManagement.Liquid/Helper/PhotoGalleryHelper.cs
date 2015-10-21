using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.LiquidEngineHelpers;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Data.Paging;
using StoreManagement.Liquid.Helper.Interfaces;

namespace StoreManagement.Liquid.Helper
{
   
    public class PhotoGalleryHelper : BaseLiquidHelper, IPhotoGalleryHelper
    {

        public StoreLiquidResult GetPhotoGalleryIndexPage(PageDesign pageDesign, List<FileManager> fileManagers)
        {
         
          
            var cats = new List<FileManagerLiquid>();
    
            foreach (var item in fileManagers)
            {
                cats.Add(new FileManagerLiquid(item, ImageWidth,ImageHeight));
            }

            object anonymousObject = new
            {
                photogalleries = LiquidAnonymousObject.GetFileManagerLiquidEnumerable(cats)
            };

            var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);


            var dic = new Dictionary<String, String>();

            dic.Add(StoreConstants.PageOutput, indexPageOutput);




            var result = new StoreLiquidResult();
            result.LiquidRenderedResult = dic;
            result.PageDesingName = pageDesign.Name;
            return result;

        }

    }
}