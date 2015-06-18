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
    public class LabelLineRepository : BaseRepository<LabelLine, int>, ILabelLineRepository
    {
        public LabelLineRepository(IStoreContext dbContext)
            : base(dbContext)
        {

        }

        public List<LabelLine> GetLabelLinesByItem(int itemId, string itemType)
        {
            var labelLines = this.FindBy(r => r.ItemId == itemId && r.ItemType == itemType).ToList();
            return labelLines;
        }

        public void DeleteLabelLinesByItem(int itemId, string itemType)
        {
            var labelLines = this.FindBy(r => r.ItemId == itemId && r.ItemType == itemType).ToList();
            foreach (var labelLine in labelLines)
            {
                Delete(labelLine);
            }
            Save();
        }

        public void SaveLabelLines(int[] labelId, int itemId, string itemType)
        {
            DeleteLabelLinesByItem(itemId, itemType);
            if (labelId != null)
            {
                foreach (var i in labelId)
                {
                    Add(new LabelLine() { ItemId = itemId, ItemType = itemType, LabelId = i });
                }
            }
            Save();
        }
    }
}
