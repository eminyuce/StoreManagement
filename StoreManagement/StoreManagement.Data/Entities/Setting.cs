using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;

namespace StoreManagement.Data.Entities
{
    public class Setting : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SettingKey { get; set; }
        public string SettingValue { get; set; }



        [IgnoreDataMember]
        public int StoreId { get; set; }
        [IgnoreDataMember]
        public string Type { get; set; }
        [IgnoreDataMember]
        public int Ordering { get; set; }
        [IgnoreDataMember]
        public string Description { get; set; }
        [IgnoreDataMember]
        public Boolean State { get; set; }
    }
}
