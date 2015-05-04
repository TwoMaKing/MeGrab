using Eagle.Core.SqlQueries.DialectProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Core.SqlQueries.Criterias
{
    public class LikeSqlCriteria : OperatorSqlCriteria
    {
        public LikeSqlCriteria(ISqlQueryDialectProvider dialectProvider, string columnName)
            : base(dialectProvider, columnName) { }

        public LikeSqlCriteria(ISqlQueryDialectProvider dialectProvider, string columnName, Func<string, string> formatter) : 
            base(dialectProvider, columnName, formatter) { }

        protected override string GetOperatorChar()
        {
            return "LIKE";
        }

    }
}
