using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GenericRepository;
using GenericRepository.EntityFramework.Enums;
using NLog;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.HelpersModel;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Service.GenericRepositories
{
    public class BaseEntityRepository : GenericBaseRepository
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

        public static async Task<List<T>> GetActiveBaseEnitiesAsync<T>(IBaseRepository<T, int> repository, int storeId, int? take, bool? isActive) where T : BaseEntity
        {
            try
            {
                Expression<Func<T, bool>> match = r2 => r2.StoreId == storeId && r2.State == (isActive.HasValue ? isActive.Value : r2.State);
                var items = repository.FindAllAsync(match, t => t.Ordering, OrderByType.Descending, take);
                var itemsResult = items;
                return await itemsResult;
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }
        }
        public static async Task<List<T>> GetBaseEnitiesAsync<T>(IBaseRepository<T, int> repository, int storeId, int? take) where T : BaseEntity
        {
            try
            {
                Expression<Func<T, bool>> match = r2 => r2.StoreId == storeId;
                var items = repository.FindAllAsync(match, t => t.Ordering, OrderByType.Descending, take);
                var itemsResult = items;
                return await itemsResult;
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }
        }
        public static List<T> GetBaseEntitiesSearchList<T>(IBaseRepository<T, int> repository, int storeId, string search) where T : BaseEntity
        {
            try
            {

                var items = repository.FindBy(r => r.StoreId == storeId);

                if (!String.IsNullOrEmpty(search.ToStr()))
                {
                    items = items.Where(r => r.Name.ToLower().Contains(search.ToLower().Trim()));
                }

                return items.OrderBy(r => r.Ordering).ThenByDescending(r => r.Id).ToList();

            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }
        }
        public static List<T> GetActiveBaseEntitiesSearchList<T>(IBaseRepository<T, int> repository, int storeId, string search) where T : BaseEntity
        {
            try
            {


                var items = repository.FindBy(r => r.StoreId == storeId && r.State);

                if (!String.IsNullOrEmpty(search.ToStr()))
                {
                    items = items.Where(r => r.Name.ToLower().Contains(search.ToLower().Trim()));
                }

                return items.OrderBy(r => r.Ordering).ThenByDescending(r => r.Id).ToList();

            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }

        }


        #endregion
    }
}
