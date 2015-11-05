using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LiquidEngineHelpers;
using StoreManagement.Data.LiquidEntities;
using StoreManagement.Liquid.Helper.Interfaces;

namespace StoreManagement.Liquid.Helper
{
    public class ContactHelper : BaseLiquidHelper, IContactHelper
    {
        public StoreLiquidResult GetContactIndexPage(PageDesign pageDesign, List<Contact> contacts)
        {
            var result = new StoreLiquidResult();
            var dic = new Dictionary<String, String>();
            result.LiquidRenderedResult = dic;
            result.PageDesingName = pageDesign.Name;
            dic.Add(StoreConstants.PageOutput, "");

            try
            {


            

                var items = new List<ContactLiquid>();
                foreach (var item in contacts)
                {

                    var i = new ContactLiquid(item, ImageWidth, ImageHeight);
                    items.Add(i);

                }


                object anonymousObject = new
                {
                    contacts = LiquidAnonymousObject.GetContactEnumerable(items)
                            
                };
                
                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign, anonymousObject);


                dic[StoreConstants.PageOutput] = indexPageOutput;


            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return result;
        }

      
    }
}