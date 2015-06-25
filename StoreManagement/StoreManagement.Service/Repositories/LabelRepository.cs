using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Data.GeneralHelper;
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

        public List<Label> GetLabelsByTypeAndCategoryAndSearch(int storeId, string labelType, int categoryId, string search)
        {
            var items =
                this.FindBy(r => r.LabelType.Equals(labelType) && r.StoreId == storeId && r.CategoryId == categoryId);

            if (!String.IsNullOrEmpty(search.ToStr()))
            {
                items = items.Where(r => r.Name.ToLower().Contains(search.ToLower().Trim()));
            }

            return items.OrderBy(r => r.Ordering).ThenByDescending(r => r.Id).ToList();
        }
    }
}
