using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.EntityFramework;
using StoreManagement.Data.Entities;

namespace StoreManagement.Service.Repositories.Interfaces
{
    public interface IContentFileRepository : IEntityRepository<ContentFile, int>
    {
        List<ContentFile> GetContentByContentId(int contentId);
        List<ContentFile> GetContentByFileManagerId(int fileManagerId);

    }

}
