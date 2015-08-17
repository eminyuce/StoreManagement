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

namespace StoreManagement.Liquid.Helper
{
    public class BrandHelper : BaseLiquidHelper
    {
        public StoreLiquidResult GetBrandsPartial(Task<List<Brand>> brandsTask, Task<PageDesign> pageDesignTask)
        {
            var dic = new Dictionary<String, String>();
            dic.Add(StoreConstants.PageOutput, "");
            try
            {
                Task.WaitAll(pageDesignTask, brandsTask);
                var pageDesign = pageDesignTask.Result;
                var brands = brandsTask.Result;

                var items = new List<BrandLiquid>();
                foreach (var item in brands)
                {

                    var blog = new BrandLiquid(item, pageDesign, ImageWidth, ImageHeight);
                    items.Add(blog);

                }

                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, new
                {
                    items = from s in items
                            select new
                            {
                                s.Brand.Name,
                                s.Brand.Description,
                                s.DetailLink
                            }
                }
                    );


                dic[StoreConstants.PageOutput] = indexPageOutput;

            }
            catch (Exception ex)
            {
                Logger.Error(ex);

            }


            var result = new StoreLiquidResult();
            result.LiquidRenderedResult = dic;
            return result;

        }


    }
}