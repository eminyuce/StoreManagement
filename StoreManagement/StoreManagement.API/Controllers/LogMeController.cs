using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Ninject;
using StoreManagement.Data;
using StoreManagement.Data.LogEntities;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.API.Controllers
{
    public class LogMeController : BaseApiController<system_logging>
    {

        public HttpResponseMessage TestLog()
        {
            var resp = new HttpResponseMessage
                {
                    Content = new StringContent("[{\"Name\":\"ABC\"},[{\"A\":\"1\"},{\"B\":\"2\"},{\"C\":\"3\"}]]", 
                    Encoding.UTF8, "application/json")
                };
            return resp;
        }


        // POST api/Default1
        public override IEnumerable<system_logging> GetAll()
        {
            throw new NotImplementedException();
        }

        public override system_logging Get(int id)
        {
            return LogRepository.GetSingle(id);
        }

        public override HttpResponseMessage Post(system_logging value)
        {

          //  Logger.Trace("Log Post is coming :"+value.log_application);
            if (ModelState.IsValid)
            {

                var logLevel = (LogLevels) Enum.Parse(typeof(LogLevels), value.log_level);
                var logLevelConfig = (LogLevels)Enum.Parse(typeof(LogLevels), ProjectAppSettings.GetWebConfigString("LogRepositoryLogLevel"));
                if (logLevel <= logLevelConfig)
                {
                    LogRepository.Add(value);
                    LogRepository.Save();
                }

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, value);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = value.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        public override HttpResponseMessage Put(int id, system_logging value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Delete(int id)
        {

            throw new NotImplementedException();
        }
    }
}
