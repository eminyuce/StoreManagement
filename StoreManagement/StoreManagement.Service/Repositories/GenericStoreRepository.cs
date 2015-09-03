using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;
using NLog;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.HelpersModel;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Service.Repositories
{
    public class GenericStoreRepository
    {


        protected static String GetDbEntityValidationExceptionDetail(DbEntityValidationException ex)
        {

            var errorMessages = (from eve in ex.EntityValidationErrors
                                 let entity = eve.Entry.Entity.GetType().Name
                                 from ev in eve.ValidationErrors
                                 select new
                                 {
                                     Entity = entity,
                                     PropertyName = ev.PropertyName,
                                     ErrorMessage = ev.ErrorMessage
                                 });

            var fullErrorMessage = string.Join("; ", errorMessages.Select(e => string.Format("[Entity: {0}, Property: {1}] {2}", e.Entity, e.PropertyName, e.ErrorMessage)));

            var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);


            return exceptionMessage;
        }



        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #region GenericMethods


        public static void ChangeGridBaseContentOrderingOrState<T>(IBaseRepository<T, int> repository, List<OrderingItem> values, String checkbox = "") where T : class, IEntity<int>
        {
            try
            {
                foreach (OrderingItem item in values)
                {
                    var t = repository.GetSingle(item.Id);
                    var baseContent = t as BaseContent;
                    if (baseContent != null)
                    {
                        if (String.IsNullOrEmpty(checkbox))
                        {
                            baseContent.Ordering = item.Ordering;
                        }
                        else if (checkbox.Equals("imagestate", StringComparison.InvariantCultureIgnoreCase))
                        {
                            baseContent.ImageState = item.State;
                        }
                        else if (checkbox.Equals("state", StringComparison.InvariantCultureIgnoreCase))
                        {
                            baseContent.State = item.State;
                        }
                        else if (checkbox.Equals("mainpage", StringComparison.InvariantCultureIgnoreCase))
                        {
                            baseContent.MainPage = item.State;
                        }
                    }
                    repository.Edit(t);
                }
                repository.Save();
            }
            catch (Exception exception)
            {
                Logger.ErrorException("ChangeGridOrderingOrState<T> :" + String.Join(",", values), exception);
            }
        }


        public static void ChangeGridBaseEntityOrderingOrState<T>(IBaseRepository<T, int> repository, List<OrderingItem> values, String checkbox = "") where T : class, IEntity<int>
        {
            try
            {
                foreach (OrderingItem item in values)
                {
                    var t = repository.GetSingle(item.Id);
                    var baseContent = t as BaseEntity;
                    if (baseContent != null)
                    {
                        if (String.IsNullOrEmpty(checkbox))
                        {
                            baseContent.Ordering = item.Ordering;
                        }
                        else if (checkbox.Equals("state", StringComparison.InvariantCultureIgnoreCase))
                        {
                            baseContent.State = item.State;
                        }

                    }
                    repository.Edit(t);
                }
                repository.Save();
            }
            catch (Exception exception)
            {
                Logger.Error(exception, "ChangeGridOrderingOrState<T> :" + String.Join(",", values), checkbox);
            }
        }
        public static void DeleteBaseEntity<T>(IBaseRepository<T, int> repository, List<string> values) where T : class, IEntity<int>
        {
            try
            {
                foreach (String v in values)
                {
                    var id = v.ToInt();
                    var item = repository.GetSingle(id);
                    repository.Delete(item);
                }
                repository.Save();
            }
            catch (DbEntityValidationException ex)
            {
                var message = GetDbEntityValidationExceptionDetail(ex);
                Logger.Error(ex, "DbEntityValidationException:" + message);
            }
            catch (Exception exception)
            {
                Logger.Error(exception, "DeleteBaseEntity :" + String.Join(",", values));
            }
        }
        #endregion
    }
}
