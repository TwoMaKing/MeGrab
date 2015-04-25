﻿using Eagle.Core.Query;
using Eagle.Core.SqlQueries;
using Eagle.Core.SqlQueries.Criterias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Eagle.Core.SqlQueries
{
    public interface ISqlQuery : IDisposable
    {
        ISqlBuilder SqlBuilder { get; }

        ISqlQuery From(string table);

        ISqlQuery InnerJoin(string joinTable, string fromKey, string joinKey);

        ISqlQuery LeftOuterJoin(string joinTable, string fromKey, string joinKey);

        ISqlQuery RightOuterJoin(string joinTable, string fromKey, string joinKey);

        ISqlQuery And(string criteria);

        ISqlQuery And(ISqlCriteria criteria);

        ISqlQuery Or(string criteria);

        ISqlQuery Or(ISqlCriteria criteria);

        ISqlQuery Filter(string column, Operator @operator, object value, bool isOr = false);

        ISqlQuery Equals(string column, object value, bool isOr = false);

        ISqlQuery NotEquals(string column, object value, bool isOr = false);

        ISqlQuery Contains(string column, object value, bool isOr = false);

        ISqlQuery StartsWith(string column, object value, bool isOr = false);

        ISqlQuery EndsWith(string column, object value, bool isOr = false);

        ISqlQuery GreaterThan(string column, object value, bool isOr = false);

        ISqlQuery GreaterThanEquals(string column, object value, bool isOr = false);

        ISqlQuery LessThan(string column, object value, bool isOr = false);

        ISqlQuery LessThanEquals(string column, object value, bool isOr = false);

        ISqlQuery In(string column, IEnumerable<object> values, bool isOr = false);

        ISqlQuery NotIn(string column, IEnumerable<object> values, bool isOr = false);

        ISqlQuery OrderBy(string column, SortOrder sortOrder);

        ISqlQuery GroupBy(string[] columns, string having = "");

        ISqlQuery Distinct();

        ISqlQuery Count(string column);

        ISqlQuery Max(string column);

        ISqlQuery Min(string column);

        ISqlQuery Sum(string column);

        ISqlQuery Select(params string[] columns);

        ISqlQuery Where(string wherePredicate, IEnumerable<object> paramValues);

        ISqlQuery Page(string identityColumn, int pageNumber, int pageSize);

        ISqlQuery Clear();

        IEnumerable<T> ToEntityList<T>(IDbConnection connection, IDbTransaction transaction = null);
    }

}
