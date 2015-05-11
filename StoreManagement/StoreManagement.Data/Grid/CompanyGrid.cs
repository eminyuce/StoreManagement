using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreManagement.Data.Entities;
using GridMvc;

namespace StoreManagement.Data.Grid
{
    public class CompanyGrid : Grid<Company>
    {
        public CompanyGrid(IQueryable<Company> items) : base(items)
        {
            Columns.Add(foo => foo.Client).Sortable(true);
            Columns.Add(foo => foo.City).Sortable(true);
            Columns.Add(foo => foo.Country).Sortable(true);
            EnablePaging = true;
            Pager.PageSize = 10;
        }
    }
}
