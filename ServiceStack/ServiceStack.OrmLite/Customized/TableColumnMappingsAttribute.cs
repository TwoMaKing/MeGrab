using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceStack.OrmLite
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TableColumnMappingsAttribute : AttributeBase
    {
        private string[] columnMappings;

        public TableColumnMappingsAttribute(string[] columnMappings)
        {
            this.columnMappings = columnMappings;
        }

        public string[] ColumnMappings
        {
            get 
            {
                return this.columnMappings;
            }
        }
    }
}
