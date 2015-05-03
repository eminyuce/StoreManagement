using StoreManagement.Data.Entities;
using StoreManagement.Data.HelpersModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.RequestModel
{
    public class CompanySearchResult
    {
        public List<Company> Companies { get; set; }
        public List<Filter> Filters { get; set; }
        public List<FilterGroup> FiltersGroups
        {
            get
            {
                var groups = new List<FilterGroup>();

                var groupNames = Filters.Select(i => i.FieldName).Distinct().ToList();
                foreach (var groupName in groupNames)
                {
                    var group = new FilterGroup(groupName);
                    group.Filters.AddRange(Filters.Where(r => r.FieldName.Equals(groupName)));
                    groups.Add(group);

                }

                return groups;

            }
        }
        public RecordsStats Stats { get; set; }
        public String Search { get; set; }
    }
}
