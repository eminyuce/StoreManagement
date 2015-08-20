using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;

namespace StoreManagement.Data.Entities
{
    public abstract class BaseFileEntity : IEntity
    {
        public int Id { get; set; }
        public int FileManagerId { get; set; }
        public bool IsMainImage { get; set; }

        public virtual FileManager FileManager { get; set; }


    }
}
