using Eagle.Core.SqlQueries.DialectProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Core.SqlQueries.Criterias
{
    public class LessThanSqlCriteria : OperatorSqlCriteria
    {
        public LessThanSqlCriteria(ISqlQueryDialectProvider dialectProvider, string columnName) : base(dialectProvider, columnName) { }

        public LessThanSqlCriteria(ISqlQueryDialectProvider dialectProvider, string columnName, string sqlSubQuery) :
            base(dialectProvider, columnName, sqlSubQuery) { }

        protected override string GetOperatorChar()
        {
            return "<";
        }
    }
}
