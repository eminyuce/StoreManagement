using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.API.Controllers
{
    public class LabelsController : BaseApiController<Label>, ILabelService
    {
        public override IEnumerable<Label> GetAll()
        {
            throw new NotImplementedException();
        }

        public override Label Get(int id)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Post(Label value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Put(int id, Label value)
        {
            throw new NotImplementedException();
        }

        public override HttpResponseMessage Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Label> GetLabelsByLabelType(string labelType)
        {
            throw new NotImplementedException();
        }

        public List<Label> GetLabelsByLabelType(int storeId, string labelType)
        {
            throw new NotImplementedException();
        }
    }
}
