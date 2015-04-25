using Eagle.Core.SqlQueries.DialectProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Core.SqlQueries.Criterias
{
    public class NotEqualSqlCriteria : OperatorSqlCriteria
    {
        public NotEqualSqlCriteria(ISqlQueryDialectProvider dialectProvider, string columnName) : base(dialectProvider, columnName) { }

        protected override string GetOperatorChar()
        {
            return "!=";
        }
    }
}
