using Eagle.Core.Query;
using Eagle.Core.SqlQueries.Criterias;
using Eagle.Core.SqlQueries.DialectProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Core.SqlQueries
{
    public interface ISqlCriteriaExpression : ISqlCriteria
    {
        ISqlCriteriaExpression Equals(string column, object value, bool isOr = false);

        ISqlCriteriaExpression Equals(string column, object value, Func<string, string> formatter, bool isOr = false);

        ISqlCriteriaExpression NotEquals(string column, object value, bool isOr = false);

        ISqlCriteriaExpression Contains(string column, object value, bool isOr = false);

        ISqlCriteriaExpression StartsWith(string column, object value, bool isOr = false);

        ISqlCriteriaExpression EndsWith(string column, object value, bool isOr = false);

        ISqlCriteriaExpression GreaterThan(string column, object value, bool isOr = false);

        ISqlCriteriaExpression GreaterThan(string column, string sqlSubQuery, bool isOr = false, params object[] queryParams);

        ISqlCriteriaExpression GreaterThanEquals(string column, object value, bool isOr = false);

        ISqlCriteriaExpression GreaterThanEquals(string column, object value, Func<string, string> formatter, bool isOr = false);

        ISqlCriteriaExpression GreaterThanEquals(string column, string sqlSubQuery, bool isOr = false, params object[] queryParams);

        ISqlCriteriaExpression LessThan(string column, object value, bool isOr = false);

        ISqlCriteriaExpression LessThan(string column, string sqlSubQuery, bool isOr = false, params object[] queryParams);

        ISqlCriteriaExpression LessThanEquals(string column, object value, bool isOr = false);

        ISqlCriteriaExpression LessThanEquals(string column, object value, Func<string, string> formatter, bool isOr = false);

        ISqlCriteriaExpression LessThanEquals(string column, string sqlSubQuery, bool isOr = false, params object[] queryParams);

        ISqlCriteriaExpression In(string column, IEnumerable<object> values, bool isOr = false);

        ISqlCriteriaExpression In(string column, string sqlSubQuery, bool isOr = false, params object[] queryParams);

        ISqlCriteriaExpression NotIn(string column, IEnumerable<object> values, bool isOr = false);

        ISqlCriteriaExpression NotIn(string column, string sqlSubQuery, bool isOr = false, params object[] queryParams);

        ISqlCriteriaExpression And(ISqlCriteria sqlCriteria);

        ISqlCriteriaExpression And(string sqlFilter, params object[] filterParams);

        ISqlCriteriaExpression Or(ISqlCriteria sqlCriteria);

        ISqlCriteriaExpression Or(string sqlFilter, params object[] filterParams);

        ISqlCriteriaExpression Where(string sqlFilter, params object[] filterParams);

        IDictionary<string, object> ParameterColumnValues { get; }

        SqlQueryDialectProviderBase DialectProvider { get; }
    }
}
