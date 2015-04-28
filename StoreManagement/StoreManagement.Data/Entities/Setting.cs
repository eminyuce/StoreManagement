﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository;

namespace StoreManagement.Data.Entities
{
    public class Setting : IEntity
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string SettingKey { get; set; }
        public string SettingValue { get; set; }
        public Boolean State { get; set; }
        public string Lang { get; set; }
    }
}
