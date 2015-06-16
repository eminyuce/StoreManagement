using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Service.Repositories
{
    public class LabelRepository : BaseRepository<Label, int>, ILabelRepository
    {
        public LabelRepository(IStoreContext dbContext)
            : base(dbContext)
        {

        }
 
        public List<Label> GetLabelsByLabelType(string labelType)
        {
            return this.FindBy(r => r.LabelType.Equals(labelType)).ToList();
        }

        public List<Label> GetLabelsByLabelType(int storeId, string labelType)
        {
            return this.FindBy(r => r.LabelType.Equals(labelType)  && r.StoreId == storeId).ToList();  
        }
    }
}
