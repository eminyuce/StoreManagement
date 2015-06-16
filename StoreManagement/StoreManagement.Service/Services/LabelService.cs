using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.Interfaces;

namespace StoreManagement.Service.Services
{
    public class LabelService : BaseService, ILabelService
    {
        public LabelService(string webServiceAddress) : base(webServiceAddress)
        {

        }

        public List<Label> GetLabelsByItemType(int itemType)
        {
            throw new NotImplementedException();
        }

        public List<Label> GetLabelsByItemType(int storeId, int itemType)
        {
            throw new NotImplementedException();
        }
    }
}
