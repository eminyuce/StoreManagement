using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;

namespace StoreManagement.Data.Entities
{
    public class Setting : BaseEntity
    {
        public string Name { get; set; }
        public string SettingKey { get; set; }
        public string SettingValue { get; set; }



   
        [IgnoreDataMember]
        public string Type { get; set; }
        [IgnoreDataMember]
        public string Description { get; set; }

        public override string ToString()
        {
            return String.Format("{2}-{0}-{1}",SettingKey,SettingValue,Id);
        }
    }
}
