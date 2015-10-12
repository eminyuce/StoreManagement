using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using StoreManagement.Service.DbContext;
using StoreManagement.Service.GenericRepositories;
using StoreManagement.Service.Repositories.Interfaces;

namespace StoreManagement.Service.Repositories
{
    public class MessageRepository : BaseRepository<Message, int>, IMessageRepository
    {
        public MessageRepository(IStoreContext dbContext) : base(dbContext)
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public List<Message> GetMessagesByStoreId(int storeId, string search)
        {
            return BaseEntityRepository.GetBaseEntitiesSearchList(this, storeId, search);
        }

        public void SaveContactFormMessage(Message message)
        {
            this.Add(message);
            this.Save();
        }
    }
}
