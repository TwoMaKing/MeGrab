﻿using Eagle.Core.SqlQueries.DialectProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Core.SqlQueries.Criterias
{
    public class NotInSqlCriteria : OperatorSqlCriteria
    {
        public NotInSqlCriteria(ISqlQueryDialectProvider dialectProvider, string columnName) : base(dialectProvider, columnName) { }

        public NotInSqlCriteria(ISqlQueryDialectProvider dialectProvider, string columnName, string sqlSubQuery) : base(dialectProvider, columnName, sqlSubQuery) { }

        protected override string GetOperatorChar()
        {
            return "NOT IN";
        }
    }
}
