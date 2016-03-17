using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Constants;
using StoreManagement.Data.GeneralHelper;
using StoreManagement.Data.HelpersModel;
using StoreManagement.Data.LogEntities;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Service.Repositories
{
    
    public class LogRepository : BaseRepository<system_logging, int>, ILogRepository
    {

 
        public LogRepository(IStoreContext dbContext)
            : base(dbContext)
        {
 
        }


        public LoggingModel GetApplicationLogs(String appName, String logLevel, int top, int skip, String search = "")
        {
            var result = new LoggingModel();

            // Create a SQL command to execute the sproc 
            using (SqlCommand command = (SqlCommand)StoreDbContext.Database.Connection.CreateCommand())
            {
                command.CommandTimeout = StoreConstants.StoreProcedureCommandTimeOut;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "log_GetApplicationLogs";
                if (!String.IsNullOrEmpty(appName.Trim()))
                {
                    command.Parameters.Add("AppName", SqlDbType.NVarChar).Value = appName;
                }
                else
                {
                    command.Parameters.Add("AppName", SqlDbType.NVarChar).Value = DBNull.Value;
                }

                if (!String.IsNullOrEmpty(search.Trim()))
                {
                    command.Parameters.Add("search", SqlDbType.NVarChar).Value = search;
                }
                else
                {
                    command.Parameters.Add("search", SqlDbType.NVarChar).Value = DBNull.Value;
                }

                if (!String.IsNullOrEmpty(logLevel.Trim()))
                {
                    command.Parameters.Add("logLevel", SqlDbType.NVarChar).Value = logLevel;
                }
                else
                {
                    command.Parameters.Add("logLevel", SqlDbType.NVarChar).Value = DBNull.Value;
                }

                command.Parameters.Add("top", SqlDbType.Int).Value = top;
                command.Parameters.Add("skip", SqlDbType.Int).Value = skip;


                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    adapter.SelectCommand = command;

                    using (DataSet dataset = new DataSet())
                    {
                        adapter.Fill(dataset);

                        var systemLoggingList = new List<system_logging>();
                        using (DataTable dt = dataset.Tables[0])
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                var systemLogging = new system_logging();
                                systemLogging.entered_date = dr["entered_date"].ToDateTime();
                                systemLogging.log_application = dr["log_application"].ToStr();
                                systemLogging.log_date = dr["log_date"].ToStr();
                                systemLogging.log_level = dr["log_level"].ToStr();
                                systemLogging.log_logger = dr["log_logger"].ToStr();
                                systemLogging.log_message = dr["log_message"].ToStr();
                                systemLogging.log_call_site = dr["log_call_site"].ToStr();
                                systemLogging.log_exception = dr["log_exception"].ToStr();
                                systemLogging.log_stacktrace = dr["log_stacktrace"].ToStr();
                                systemLoggingList.Add(systemLogging);
                            }
                        }

                        result.SystemLoggingList = systemLoggingList;

                        RecordsStats stats = new RecordsStats();
                        using (DataTable dt = dataset.Tables[1])
                        {

                            foreach (DataRow dr in dt.Rows)
                            {

                                stats.RecordFirst = dr["RecordFirst"].ToInt();
                                stats.RecordLast = dr["RecordLast"].ToInt();
                                stats.RecordsTotal = dr["RecordsTotal"].ToInt();
                                stats.RecordCount = dr["recordCount"].ToInt();


                            }
                        }
                        result.RecordsStats = stats;

                    }// dataset
                } //dataAdapter
            }//command

            return result;

        }
        public List<AppNames> GetApplicationNames()
        {
            var result = new List<AppNames>();

            // Create a SQL command to execute the sproc 
            using (SqlCommand command = (SqlCommand)StoreDbContext.Database.Connection.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = StoreConstants.StoreProcedureCommandTimeOut;
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    command.CommandText = "log_GetApplicationNames";
                    adapter.SelectCommand = command;

                    using (DataSet dataset = new DataSet())
                    {
                        adapter.Fill(dataset);


                        using (DataTable dt = dataset.Tables[0])
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                AppNames appNames = new AppNames();
                                appNames.Log_application = dr["Log_application"].ToStr();
                                appNames.LastActivity = dr["LastActivity"].ToDateTime();
                                appNames.LastError = dr["LastError"].ToDateTime();
                                appNames.TotalRecords = dr["TotalRecords"].ToInt();
                                result.Add(appNames);
                            }
                        }

                    }// dataset
                } //dataAdapter
            }//command

            return result;
        }


        public void DeleteLogs(string application = "", String logLevel="")
        {
            StoreDbContext.Database.Connection.Open();
            // Create a SQL command to execute the sproc 
            using (SqlCommand command = (SqlCommand)StoreDbContext.Database.Connection.CreateCommand())
            {
                if (String.IsNullOrEmpty(application))
                {
                    command.Parameters.Add("AppName", SqlDbType.NVarChar).Value = DBNull.Value;
                }
                else
                {
                    command.Parameters.Add("AppName", SqlDbType.NVarChar).Value = application;
                }
                if (String.IsNullOrEmpty(logLevel))
                {
                    command.Parameters.Add("logLevel", SqlDbType.NVarChar).Value = DBNull.Value;
                }
                else
                {
                    command.Parameters.Add("logLevel", SqlDbType.NVarChar).Value = logLevel;
                }
                command.CommandText = "log_DeleteApplicationLogs";
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = StoreConstants.StoreProcedureCommandTimeOut;
                command.ExecuteNonQuery();
            }//command
            StoreDbContext.Database.Connection.Close();
        }

        public List<DataTable> GetTotalTablesSpace()
        {
            var result = new List<DataTable>();

            // Create a SQL command to execute the sproc 
            using (SqlCommand command = (SqlCommand)StoreDbContext.Database.Connection.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 200000;
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    command.CommandText = "TotalTablesSpace";
                    adapter.SelectCommand = command;

                    using (DataSet dataset = new DataSet())
                    {
                        adapter.Fill(dataset);
                        
                        using (DataTable dt = dataset.Tables[0])
                        {
                            result.Add(dt);
                        }

                    }// dataset
                } //dataAdapter
            }//command

            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
