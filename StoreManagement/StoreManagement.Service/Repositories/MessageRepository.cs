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
    public class MessageRepository : BaseRepository<Message, int>, IMessageRepository
    {
        public MessageRepository(IStoreContext dbContext) : base(dbContext)
        {
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void SaveContactFormMessage(Message message)
        {
            this.Add(message);
            this.Save();
        }
    }
}
