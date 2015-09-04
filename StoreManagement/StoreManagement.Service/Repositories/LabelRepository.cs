using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Constants;
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

        public List<Label> GetActiveLabels(int storeId)
        {
            return GenericStoreRepository.GetActiveBaseEntitiesSearchList(this, storeId, "");
        }

        public Label GetLabelByName(string label, int storeId)
        {
            return this.FindBy(r => r.StoreId == storeId && r.Name.Equals(label, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }

        public List<Label> GetStoreLabels(int storeId)
        {
            return GenericStoreRepository.GetBaseEntitiesSearchList(this, storeId, "");
        }

        public List<Label> GetLabelsByStoreId(int storeId, string searchKey)
        {
            return GenericStoreRepository.GetBaseEntitiesSearchList(this, storeId, searchKey);
        }

        public Task<List<Label>> GetLabelsByItemTypeId(int storeId, int itemId, string itemType)
        {
            var labelIds =
                StoreDbContext.LabelLines.Where(
                    r => r.ItemId == itemId && itemType.Equals(itemType, StringComparison.InvariantCultureIgnoreCase))
                              .ToList();
            var labelidList = labelIds.Select(r1 => r1.LabelId);
            var items = this.FindAllAsync(r => r.StoreId == storeId && labelidList.Contains(r.Id), null);


            return items;


        }
    }
}
