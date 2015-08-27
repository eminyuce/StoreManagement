using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;
using StoreManagement.Data.LogEntities;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface ILogRepository : IBaseRepository<system_logging, int>
    {
        LoggingModel GetApplicationLogs(String appName, String logLevel, int top, int skip, String search = "");
        List<AppNames> GetApplicationNames();
        void DeleteLogs(string application = "", String logLevel="");
        List<DataTable> GetTotalTablesSpace();
    }
}
