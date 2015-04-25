using Eagle.Core.SqlQueries.DialectProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Core.SqlQueries.Criterias
{
    public class LessThanEqualSqlCriteria : OperatorSqlCriteria
    {
        public LessThanEqualSqlCriteria(ISqlQueryDialectProvider dialectProvider, string columnName) : base(dialectProvider, columnName) { }

        public LessThanEqualSqlCriteria(ISqlQueryDialectProvider dialectProvider, string columnName, string sqlSubQuery) :
            base(dialectProvider, columnName, sqlSubQuery) { }

        protected override string GetOperatorChar()
        {
            return "<=";
        }
    }
}
