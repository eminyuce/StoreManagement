using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework.Enums;
using StoreManagement.Data;
using StoreManagement.Data.CacheHelper;
using StoreManagement.Data.Constants;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.HelpersModel;
using StoreManagement.Data.Paging;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.GenericRepositories;
using StoreManagement.Service.Interfaces;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Service.Repositories
{
    public class ProductRepository : BaseRepository<Product, int>, IProductRepository
    {

        private static readonly TypedObjectCache<List<Product>> ProductCache = new TypedObjectCache<List<Product>>("ProductsCache");
        private static readonly TypedObjectCache<StorePagedList<Product>> ProductCacheStorePagedList = new TypedObjectCache<StorePagedList<Product>>("StorePagedListProduct");


        public ProductRepository(IStoreContext dbContext)
            : base(dbContext)
        {

        }

        public Product GetProductsById(int productId)
        {
            return this.GetSingleIncluding(productId, r => r.ProductFiles.Select(r1 => r1.FileManager));
        }

        public List<Product> GetProductByType(string typeName)
        {
            return
               this.FindBy(r => r.Type.Equals(typeName, StringComparison.InvariantCultureIgnoreCase))
                   .ToList();
        }

        public List<Product> GetProductByType(int storeId, string typeName)
        {
            return
               this.FindBy(
                   r => r.StoreId == storeId && r.Type.Equals(typeName, StringComparison.InvariantCultureIgnoreCase))
                   .ToList();
        }

        public List<Product> GetProductByTypeAndCategoryId(int storeId, string typeName, int categoryId)
        {
            return this.GetAllIncluding(r1 => r1.ProductFiles.Select(r2 => r2.FileManager)).Where(
                       r => r.StoreId == storeId &&
                           r.Type.Equals(typeName, StringComparison.InvariantCultureIgnoreCase) &&
                           r.ProductCategoryId == (categoryId > 0 ? categoryId : r.ProductCategoryId))
                       .ToList();
        }

        public List<Product> GetProductByTypeAndCategoryId(int storeId, string typeName, int categoryId, string search, bool? isActive)
        {

            Expression<Func<Product, bool>> match = r2 => r2.StoreId == storeId
                && r2.State == (isActive.HasValue ? isActive.Value : r2.State)
                && r2.ProductCategoryId == (categoryId > 0 ? categoryId : r2.ProductCategoryId) &&
                     r2.Type.Equals(typeName, StringComparison.InvariantCultureIgnoreCase);


            var predicate = PredicateBuilder.Create<Product>(match);
            if (!String.IsNullOrEmpty(search.ToStr()))
            {
                predicate = predicate.And(r => r.Name.ToLower().Contains(search.ToLower().Trim()));
            }


            var products = this.FindBy(predicate);

            return products.OrderBy(r => r.Ordering).ThenByDescending(r => r.Id).ToList();

        }

        public List<Product> GetProductByTypeAndCategoryIdFromCache(int storeId, string typeName, int categoryId)
        {
            String key = String.Format("GetProductByTypeAndCategoryIdFromCache-{0}-{1}-{2}", storeId, typeName, categoryId);
            List<Product> items = null;
            ProductCache.TryGet(key, out items);

            if (items == null)
            {
                items = GetProductByTypeAndCategoryId(storeId, typeName, categoryId);
                ProductCache.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("Products_CacheAbsoluteExpiration_Minute", 10)));
            }

            return items;
        }

        public StorePagedList<Product> GetProductsCategoryId(int storeId, int? categoryId, string typeName, bool? isActive, int page,
                                                    int pageSize)
        {
            String key = String.Format("GetContentsCategoryId-{0}-{1}-{2}-{3}-{4}-{5}", storeId, typeName, categoryId, isActive.HasValue ? isActive.Value.ToStr() : "", page, pageSize);
            StorePagedList<Product> items = null;
            ProductCacheStorePagedList.TryGet(key, out items);

            if (items == null)
            {
                var returnList =
                        this.GetAllIncluding(r => r.ProductFiles.Select(r1 => r1.FileManager)).Where(r2 => r2.StoreId == storeId && r2.Type.Equals(typeName, StringComparison.InvariantCultureIgnoreCase));


                if (categoryId.HasValue)
                {
                    returnList = returnList.Where(r => r.ProductCategoryId == categoryId.Value);
                }
                if (isActive.HasValue)
                {
                    returnList = returnList.Where(r => r.State == isActive);
                }

                var cat = returnList.OrderByDescending(r => r.Id).ToList();
                items = new StorePagedList<Product>(cat.Skip((page - 1) * pageSize).Take(pageSize).ToList(), page, pageSize, cat.Count());
                //  items = new PagedList<Content>(cat, page, cat.Count());
                //  items = (PagedList<Content>) cat.ToPagedList(page, pageSize);
                ProductCacheStorePagedList.Set(key, items, MemoryCacheHelper.CacheAbsoluteExpirationPolicy(ProjectAppSettings.GetWebConfigInt("Products_CacheAbsoluteExpiration_Minute", 10)));
            }

            return items;
        }

        public Product GetProductWithFiles(int id)
        {
            return this.GetAllIncluding(r2 => r2.ProductFiles.Select(r3 => r3.FileManager)).FirstOrDefault(r1 => r1.Id == id);
        }

        public async Task<StorePagedList<Product>> GetProductsCategoryIdAsync(int storeId, int? categoryId, string typeName, bool? isActive, int page, int pageSize, string search, string filters)
        {

            int? catId = categoryId.HasValue && categoryId > 0 ? categoryId : null;
            Expression<Func<Product, bool>> match = r2 => r2.StoreId == storeId && r2.State == (isActive.HasValue ? isActive.Value : r2.State) && r2.ProductCategoryId == (catId.HasValue ? catId.Value : r2.ProductCategoryId);
            Expression<Func<Product, object>> includeProperties = r => r.ProductFiles.Select(r1 => r1.FileManager);
            var predicate = PredicateBuilder.Create<Product>(match);
            if (!String.IsNullOrEmpty(search.ToStr()))
            {
                predicate = predicate.And(r => r.Name.ToLower().Contains(search.ToLower().Trim()));
            }
            if (!String.IsNullOrEmpty(filters.ToStr()))
            {

            }
            var items = await this.FindAllIncludingAsync(predicate, page, pageSize, r => r.Ordering, OrderByType.Descending, includeProperties);
            var totalItemNumber = await this.CountAsync(predicate);


            var task = Task.Factory.StartNew(() =>
            {

                StorePagedList<Product> resultItems = new StorePagedList<Product>(items, page, pageSize, totalItemNumber);
                return resultItems;
            });
            var result = await task;

            return result;
        }

        public async Task<Product> GetProductsByIdAsync(int productId)
        {
            Expression<Func<Product, object>> includeProperties = r => r.ProductFiles.Select(r1 => r1.FileManager);
            return await this.GetSingleIncludingAsync(productId, includeProperties);
        }

        public async Task<List<Product>> GetMainPageProductsAsync(int storeId, int? take)
        {
            try
            {
                Expression<Func<Product, bool>> match = r2 => r2.StoreId == storeId && r2.State && r2.MainPage;
                Expression<Func<Product, object>> includeProperties = r => r.ProductFiles.Select(r1 => r1.FileManager);
                var items = this.FindAllIncludingAsync(match, take, null, t => t.Ordering, OrderByType.Descending, includeProperties);

                var itemsResult = items;

                return await itemsResult;

            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }
        }

        public async Task<List<Product>> GetProductsAsync(int storeId, int? take, bool? isActive)
        {
            try
            {
                Expression<Func<Product, bool>> match = r2 => r2.StoreId == storeId && r2.State == (isActive.HasValue ? isActive.Value : r2.State);
                Expression<Func<Product, object>> includeProperties = r => r.ProductFiles.Select(r1 => r1.FileManager);
                var items = this.FindAllIncludingAsync(match, take, null, t => t.Ordering, OrderByType.Descending, includeProperties);

                var itemsResult = items;

                return await itemsResult;

            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }
        }

        public async Task<List<Product>> GetProductByTypeAndCategoryIdAsync(int storeId, int categoryId, int? take, int? excludedProductId)
        {
            try
            {
                Expression<Func<Product, bool>> match = r2 => r2.StoreId == storeId && r2.State && r2.ProductCategoryId == categoryId && r2.Id
                    != (excludedProductId.HasValue ? excludedProductId.Value : r2.Id);
                Expression<Func<Product, object>> includeProperties = r => r.ProductFiles.Select(r1 => r1.FileManager);
                var items = this.FindAllIncludingAsync(match, take, null, t => t.Ordering, OrderByType.Descending, includeProperties);

                var itemsResult = items;

                return await itemsResult;

            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }

        }

        public async Task<List<Product>> GetProductsByBrandAsync(int storeId, int brandId, int? take, int? excludedProductId)
        {
            try
            {
                int excludedProductId2 = excludedProductId ?? 0;
                Expression<Func<Product, bool>> match = r2 => r2.StoreId == storeId && r2.State && r2.BrandId == brandId && r2.Id != excludedProductId2;
                var items = this.FindAllIncludingAsync(match, take, null, t => t.Ordering, OrderByType.Descending, r => r.ProductFiles.Select(r1 => r1.FileManager));

                var itemsResult = items;
                var rt = await itemsResult;

                return rt;
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return new List<Product>();
            }
        }

        public async Task<List<Product>> GetProductsByRetailerAsync(int storeId, int retailerId, int? take, int? excludedProductId)
        {
            try
            {
                int excludedProductId2 = excludedProductId ?? 0;
                Expression<Func<Product, bool>> match = r2 => r2.StoreId == storeId && r2.State
                    && r2.RetailerId == retailerId
                    && r2.Id != excludedProductId2;
                var items = this.FindAllIncludingAsync(match, take, null, t => t.Ordering, OrderByType.Descending, r => r.ProductFiles.Select(r1 => r1.FileManager));

                var itemsResult = items;
                var rt = await itemsResult;

                return rt;
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return new List<Product>();
            }
        }

        public async Task<List<Product>> GetProductsByProductType(int storeId, int? categoryId, int? brandId, int? retailerId, string productType, int page, int pageSize, bool? isActive, string functionType, int? excludedProductId)
        {
            try
            {
                Expression<Func<Product, bool>> match = r2 => r2.StoreId == storeId
                    && r2.State == (isActive ?? r2.State)
                    && r2.ProductCategoryId == (categoryId ?? r2.ProductCategoryId)
                    && r2.BrandId == (brandId ?? r2.BrandId)
                 && r2.RetailerId == (retailerId ?? r2.RetailerId)
                    && r2.Id != excludedProductId
                &&
                r2.ProductFiles.Any();

                Expression<Func<Product, object>> includeProperties = r => r.ProductFiles.Select(r1 => r1.FileManager);


                var predicate = PredicateBuilder.Create<Product>(match);
                Expression<Func<Product, int>> keySelector = t => t.Id;

                if (functionType.Equals("mainrandom", StringComparison.InvariantCultureIgnoreCase))
                {
                    Expression<Func<Product, Guid>> keySelector2 = t => Guid.NewGuid();
                    predicate = predicate.And(r => r.MainPage);
                    var itemsRandom = this.FindAllIncludingAsync(predicate,
                        page, pageSize,
                        keySelector2,
                        OrderByType.Descending, includeProperties);
                    return await itemsRandom;
                }
                else if (functionType.Equals("random", StringComparison.InvariantCultureIgnoreCase))
                {
                    Expression<Func<Product, Guid>> keySelector2 = t => Guid.NewGuid();
                    var itemsRandom = this.FindAllIncludingAsync(predicate, page, pageSize, keySelector2, OrderByType.Descending, includeProperties);
                    return await itemsRandom;
                }
                else if (functionType.Equals("normal", StringComparison.InvariantCultureIgnoreCase))
                {
                    keySelector = t => t.Ordering;
                }
                else if (functionType.Equals("popular", StringComparison.InvariantCultureIgnoreCase))
                {
                    keySelector = t => t.TotalRating;
                }
                else if (functionType.Equals("recent", StringComparison.InvariantCultureIgnoreCase))
                {
                    keySelector = t => t.Id;
                }
                else if (functionType.Equals("main", StringComparison.InvariantCultureIgnoreCase))
                {
                    keySelector = t => t.Ordering;
                    predicate = predicate.And(r => r.MainPage);
                }
                else if (functionType.Equals("discount", StringComparison.InvariantCultureIgnoreCase))
                {
                    predicate = predicate.And(r => r.Discount > 0);

                    var items2 = this.FindAllIncludingAsync(predicate, page, pageSize, t => t.Discount, OrderByType.Descending, includeProperties);
                    return await items2;
                }

                var items = this.FindAllIncludingAsync(predicate, page, pageSize, keySelector, OrderByType.Descending, includeProperties);
                return await items;
            }
            catch (Exception exception)
            {
                Logger.Error(exception, exception.StackTrace, storeId, categoryId, brandId, productType,
                                                    page, pageSize, isActive, functionType);
                return null;
            }
        }


        public List<Product> GetMainPageProducts(int storeId, int? take)
        {
            try
            {
                Expression<Func<Product, bool>> match = r2 => r2.StoreId == storeId && r2.State && r2.MainPage;
                var items = this.FindAllIncludingAsync(match, take, null, t => t.Ordering, OrderByType.Descending, r => r.ProductFiles.Select(r1 => r1.FileManager));
                Task.WaitAll(items);
                return items.Result;
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                return null;
            }

        }
        public List<Product> GetProductsByStoreId(int storeId, String searchKey)
        {
            return BaseEntityRepository.GetBaseEntitiesSearchList(this, storeId, searchKey);
        }

        public async Task<ProductsSearchResult> GetProductsSearchResult(int storeId, string search, String filters, int top, int skip, bool isAdmin)
        {
            var fltrs = FilterHelper.ParseFiltersFromString(filters);
            ProductsSearchResult t = await Task.Run(() => this.GetProductsSearchResult(storeId, search, fltrs, top, skip, isAdmin));
            return t;
        }

        public static ItemType ProductsItem
        {
            get
            {
                return new ItemType()
                {
                    Name = "Products/Products Directory",
                    Type = typeof(Product),
                    SearchAction = "Index",
                    Controller = "Products",
                    ItemTypeID = 1
                };

            }
        }
        private ProductsSearchResult GetProductsSearchResult(
           int storeId, 
           string search,
           List<Filter> filters,
           int top,
           int skip,
           Boolean isAdmin = false)
        {
            var searchResult = new ProductsSearchResult();

            String commandText = @"SearchProducts";
            var commandType = CommandType.StoredProcedure;
            var parameterList = new List<SqlParameter>();
            var dtFilters = new DataTable("med_tpt_Filter");

            dtFilters.Columns.Add("FieldName");
            dtFilters.Columns.Add("ValueFirst");
            dtFilters.Columns.Add("ValueLast");

            if (filters != null && filters.Any())
            {
                foreach (var filter in filters)
                {
                    DataRow dr = dtFilters.NewRow();
                    dr["FieldName"] = filter.FieldName;
                    dr["ValueFirst"] = filter.ValueFirst;
                    dr["ValueLast"] = filter.ValueLast;
                    dtFilters.Rows.Add(dr);
                }
            }

            parameterList.Add(DatabaseUtility.GetSqlParameter("IsAdmin", isAdmin, SqlDbType.Bit));
            parameterList.Add(DatabaseUtility.GetSqlParameter("storeId", storeId, SqlDbType.Int));
            parameterList.Add(DatabaseUtility.GetSqlParameter("search", search.ToStr(), SqlDbType.NVarChar));
            parameterList.Add(DatabaseUtility.GetSqlParameter("filter", dtFilters, SqlDbType.Structured));
            parameterList.Add(DatabaseUtility.GetSqlParameter("top", top, SqlDbType.Int));
            parameterList.Add(DatabaseUtility.GetSqlParameter("skip", skip, SqlDbType.Int));
            DatabaseUtility.SqlCommandTimeout = StoreConstants.StoreProcedureCommandTimeOut;
            DataSet dataSet = DatabaseUtility.ExecuteDataSet((SqlConnection)StoreDbContext.Database.Connection, commandText, commandType, parameterList.ToArray());
            if (dataSet.Tables.Count > 0)
            {
                var productCategories = new List<ProductCategory>();
                using (DataTable dt = dataSet.Tables[0])
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var item = GetProductCategoriesFromDataRow(dr);
                        productCategories.Add(item);
                    };
                }
                searchResult.ProductCategories = productCategories;

                var products = new List<Product>();
                using (DataTable dt = dataSet.Tables[1])
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var item = GetProductsFromDataRow(dr);
                        products.Add(item);
                    };
                }
                searchResult.Products = products;

                using (DataTable dt = dataSet.Tables[2])
                {
                    var m = new List<Filter>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        var item = GetFilterFromDataRow(dr);
                        item.OwnerType = ProductsItem;
                        m.Add(item);
                    }
                    searchResult.Filters = m;
                }
                using (DataTable dt = dataSet.Tables[3])
                {
                    var stats = new RecordsStats();
                    foreach (DataRow dr in dt.Rows)
                    {
                        stats = GetRecordsStatsFromDataRow(dr);
                        stats.OwnerType = ProductsItem;
                    }
                    searchResult.Stats = stats;
                }
            }
            searchResult.PageSize = top;
            return searchResult;
        }
        private static ProductCategory GetProductCategoriesFromDataRow(DataRow dr)
        {
            var item = new ProductCategory();

            item.Id = dr["Id"].ToInt();
            item.StoreId = dr["StoreId"].ToInt();
            item.ParentId = dr["ParentId"].ToInt();
            item.Ordering = dr["Ordering"].ToInt();
            item.CategoryType = dr["CategoryType"].ToStr();
            item.Name = dr["Name"].ToStr();
            item.Description = dr["Description"].ToStr();
            item.State = dr["State"].ToBool();
            item.CreatedDate = dr["CreatedDate"].ToDateTime();
            item.UpdatedDate = dr["UpdatedDate"].ToDateTime();
            item.ApiCategoryId = dr["ApiCategoryId"].ToStr();
            item.ApiCategoryParentId = dr["ApiCategoryParentId"].ToStr();
            return item;
        }

        private static Filter GetFilterFromDataRow(DataRow dr)
        {
            var item = new Filter();
            item.FieldName = dr["FieldName"].ToStr();
            item.ValueFirst = dr["ValueFirst"].ToStr();
            item.ValueLast = dr["ValueLast"].ToStr();
            item.Cnt = dr["Cnt"].ToInt();

            return item;
        }
        private static RecordsStats GetRecordsStatsFromDataRow(DataRow dr)
        {
            var stats = new RecordsStats();
            stats.RecordFirst = dr["RecordFirst"].ToInt();
            stats.RecordLast = dr["RecordLast"].ToInt();
            stats.RecordsTotal = dr["RecordsTotal"].ToInt();
            stats.RecordCount = dr["recordCount"].ToInt();
            return stats;
        }

        private static Product GetProductsFromDataRow(DataRow dr)
        {
            var item = new Product();

            item.Id = dr["Id"].ToInt();
            item.StoreId = dr["StoreId"].ToInt();
            item.ProductCategoryId = dr["ProductCategoryId"].ToInt();
            item.BrandId = dr["BrandId"].ToInt();
            item.RetailerId = dr["RetailerId"].ToInt();
            item.ProductCode = dr["ProductCode"].ToStr();
            item.Name = dr["Name"].ToStr();
            item.Description = dr["Description"].ToStr();
            item.Type = dr["Type"].ToStr();
            item.MainPage = dr["MainPage"].ToBool();
            item.State = dr["State"].ToBool();
            item.Ordering = dr["Ordering"].ToInt();
            item.CreatedDate = dr["CreatedDate"].ToDateTime();
            item.ImageState = dr["ImageState"].ToBool();
            item.UpdatedDate = dr["UpdatedDate"].ToDateTime();
            item.Price = dr["Price"].ToFloat();
            item.Discount = dr["Discount"].ToFloat();
            item.UnitsInStock = dr["UnitsInStock"].ToInt();
            item.TotalRating = dr["TotalRating"].ToInt();
            item.VideoUrl = dr["VideoUrl"].ToStr();
            return item;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
