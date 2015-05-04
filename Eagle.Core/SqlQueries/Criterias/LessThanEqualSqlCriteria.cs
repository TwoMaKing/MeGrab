﻿using Eagle.Core.SqlQueries.DialectProvider;
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

        public LessThanEqualSqlCriteria(ISqlQueryDialectProvider dialectProvider, string columnName, Func<string, string> formatter) : 
            base(dialectProvider, columnName, formatter) { }

        protected override string GetOperatorChar()
        {
            return "<=";
        }
    }
}
