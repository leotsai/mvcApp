using System;
using System.Data;

namespace MvcApp.Core.Extensions
{
    public static class DataRowExtensions
    {
        public static T Get<T>(this DataRow row, string property)
        {
            try
            {
                return row.Field<T>(property);
            }
            catch (Exception ex)
            {
                if (row.Table.Columns.Contains(property) == false)
                {
                    throw new KnownException(property + " 列名不存在");
                }
                throw new KnownException($"DataRow.Get, {property}:" + row[property]+", " + ex.Message);
            }
        }
    }
}
