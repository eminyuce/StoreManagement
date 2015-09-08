using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace StoreManagement.Service.GenericRepositories
{
    public abstract class GenericBaseRepository
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    }
}
