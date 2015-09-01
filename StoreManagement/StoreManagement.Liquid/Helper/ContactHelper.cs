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
        public StoreLiquidResult GetContactIndexPage(Task<PageDesign> pageDesignTask, Task<List<Contact>> contactsTask)
        {
            var result = new StoreLiquidResult();
            var dic = new Dictionary<String, String>();
            result.LiquidRenderedResult = dic;
            dic.Add(StoreConstants.PageOutput, "");

            try
            {


                Task.WaitAll(pageDesignTask, contactsTask);
                var pageDesign = pageDesignTask.Result;
                var contacts = contactsTask.Result;

                if (pageDesign == null)
                {
                    throw new Exception("PageDesing is null");
                }


                var items = new List<ContactLiquid>();
                foreach (var item in contacts)
                {

                    var i = new ContactLiquid(item, pageDesign, ImageWidth, ImageHeight);
                    items.Add(i);

                }


                object anonymousObject = new
                {
                    items = from s in items
                            select new
                            {
                                s.Contact.Name,
                                s.Contact.Title,
                                s.Contact.PhoneCell,
                                s.Contact.PhoneWork,
                                s.Contact.Email
                            }
                            
                };

                var indexPageOutput = LiquidEngineHelper.RenderPage(pageDesign.PageTemplate, anonymousObject);


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