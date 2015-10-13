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
    public class ProductAttributeRelationRepository : BaseRepository<ProductAttributeRelation, int>, IProductAttributeRelationRepository
    {
        public ProductAttributeRelationRepository(IStoreContext dbContext) : base(dbContext)
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
