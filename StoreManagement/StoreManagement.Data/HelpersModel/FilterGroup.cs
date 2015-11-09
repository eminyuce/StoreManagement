using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.LiquidEntities;


namespace StoreManagement.Data.HelpersModel
{
    public class FilterGroup : BaseDrop
    {
        public string FieldName { get; set; }

        public string FieldNameFull
        {
            get
            {
                switch (FieldName)
                {
                    case "EmploymentType":
                        return "Employment Type";
                    case "Country":
                        return "Country";
                    case "Experience":
                        return "Experience";
                    case "Category":
                        return "Category";
                    case "State":
                        return "State";
                    case "City":
                        return "City";
                    case "GT":
                        return "Gross Tonnage";
                    default:
                        return FieldName;
                }
            }

        }


        private List<Filter> _filters = new List<Filter>();

        public List<Filter> Filters
        {
            get { return _filters; }
            set { _filters = value; }
        }

        public List<Filter> FiltersHidden
        {
            get
            {
                if (_filters.Count > ProjectAppSettings.MaxItemsCountInFilter)
                {
                    return
                        _filters.OrderByDescending(i => i.Cnt)
                                .Take(ProjectAppSettings.MaxItemsCountInFilter)
                                .OrderBy(i => i.Text)
                                .ToList();
                }
                else
                {
                    return _filters;
                }

            }

        }



        public FilterGroup(string fieldName)
        {
            FieldName = fieldName;
        }
    }
}
