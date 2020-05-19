using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Data.Extensions
{
    public static class ParametersExtensions
    {
        public static DataTable AsDataTableParam<T>(this IEnumerable<T> data)
        {
            var tableAsParam = new DataTable();
            tableAsParam.Columns.Add("Id");

            if (data != null)
            {
                foreach (var item in data)
                {
                    tableAsParam.Rows.Add(item);
                }
            }
            return tableAsParam;
        }
    }
}
