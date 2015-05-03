using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Data.RequestModel;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Filter = StoreManagement.Data.HelpersModel.Filter;
using StoreManagement.Data.GeneralHelper;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using StoreManagement.Data.HelpersModel;
using System.Data.EntityClient;
using System.Data.Common;
using System;
using System.Reflection;

namespace StoreManagement.Service.Repositories
{
    public class CompanyRepository : EntityRepository<Company, int>, ICompanyRepository
    {
        private StoreContext dbContext;
        public CompanyRepository(IStoreContext dbContext)
            : base(dbContext)
        {
            this.dbContext = (StoreContext)dbContext;
        }

        public static ItemType Company
        {
            get
            {
                return new ItemType()
                {
                    Name = "Companies/Companies Directory",
                    Type = typeof(Company),
                    SearchAction = "CompaniesSearch",
                    Controller = "Companies",
                    ItemTypeID = 1
                };

            }
        }

        public CompanySearchResult GetCompanySearchResult(string search = "", List<Filter> filters = null, int take = 20, int skip = 0)
        {
            var result = new CompanySearchResult();


            DataTable dtFilters = new DataTable("med_tpt_Filter");

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

            // Create a SQL command to execute the sproc 
            using (SqlCommand cmd = (SqlCommand)dbContext.Database.Connection.CreateCommand())
            {
                cmd.CommandText = "dbo.SearchCompanies";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("filter", SqlDbType.Structured).Value = dtFilters;
                cmd.Parameters.Add("search", SqlDbType.NVarChar).Value = search;
                cmd.Parameters.Add("top", SqlDbType.Int).Value = take;
                cmd.Parameters.Add("skip ", SqlDbType.Int).Value = skip;


                try
                {

                    dbContext.Database.Connection.Open();
                    var reader = cmd.ExecuteReader();
                    List<Company> companyList = MapToListHelper.DataReaderMapToList<Company>(reader);
                    result.Companies = companyList;
                    reader.NextResult();
                    List<Filter> filterResult = MapToListHelper.DataReaderMapToList<Filter>(reader);
                    result.Filters = filterResult;
                    filterResult.ForEach(r => r.OwnerType = Company);
                    reader.NextResult();
                    List<RecordsStats> stats = MapToListHelper.DataReaderMapToList<RecordsStats>(reader);
                    result.Stats = stats.Any() ? stats.FirstOrDefault() : null;
                    if (result.Stats != null)
                    {
                        result.Stats.OwnerType = Company;
                    }
                }
                finally
                {
                    dbContext.Database.Connection.Close();
                }
            }
           

          

            return result;

        }

     

    }


}


