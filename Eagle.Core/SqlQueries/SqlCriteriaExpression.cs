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
    public class SqlCriteriaExpression : ISqlCriteriaExpression
    {
        private SqlQueryDialectProviderBase dialectProvider;

        private ISqlCriteria globalSqlCriteria = null;

        private IDictionary<string, object> parameterColumnValues = new Dictionary<string, object>();

        private IList<object> parameterValues = new List<object>();

        public SqlCriteriaExpression(SqlQueryDialectProviderBase dialectProvider) 
        {
            this.dialectProvider = dialectProvider;
        }

        protected ISqlCriteriaExpression Filter(string column, Operator @operator, string sqlSubQuery, bool isOr = false, params object[] queryParams)
        {
            OperatorSqlCriteria sqlCriteria = OperatorSqlCriteria.Create(this.dialectProvider, column, @operator, sqlSubQuery);

            if (isOr)
            {
                this.Or(sqlCriteria);
            }
            else
            {
                this.And(sqlCriteria);
            }
            
            string[] parameterNames = this.dialectProvider.DiscoverParams(sqlSubQuery);
            
            if ((parameterNames != null && 
                 parameterNames.Length > 0) &&
                (queryParams == null || 
                 queryParams.Length.Equals(0)))
            {
                throw new ArgumentException("The sub query sql statement contains parameter names. The parameter value cannot be null.");
            }

            if (parameterNames != null &&
                parameterNames.Length > 0 &&
                queryParams != null &&
                queryParams.Length > 0)
            {
                for (int parameterIndex = 0; parameterIndex < parameterNames.Length; parameterIndex++)
                {
                    string parameterColumnName = ParameterColumnCache.Instance.GetParameterColumn(parameterNames[parameterIndex]);
 
                    object parameterValue = queryParams[parameterIndex];

                    this.parameterColumnValues.Add(parameterColumnName, parameterValue);

                    this.parameterValues.Add(parameterValue);
                }
            }

            return this;
        }

        protected ISqlCriteriaExpression Filter(string column, Operator @operator, object value, bool isOr = false)
        {
            OperatorSqlCriteria sqlCriteria = OperatorSqlCriteria.Create(this.dialectProvider, column, @operator);

            if (this.globalSqlCriteria == null)
            {
                this.globalSqlCriteria = sqlCriteria;
            }
            else
            {
                if (isOr)
                {
                    this.Or(sqlCriteria);
                }
                else
                {
                    this.And(sqlCriteria);
                }
            }

            this.parameterColumnValues.Add(sqlCriteria.ParameterColumnName, value);
            this.parameterValues.Add(value);

            return this;
        }

        public ISqlCriteriaExpression Equals(string column, object value, bool isOr = false)
        {
            return this.Filter(column, Operator.Equal, value, isOr);
        }

        public ISqlCriteriaExpression NotEquals(string column, object value, bool isOr = false)
        {
            return this.Filter(column, Operator.NotEqual, value, isOr);
        }

        public ISqlCriteriaExpression Contains(string column, object value, bool isOr = false) 
        {
             return this.Filter(column, Operator.Contains, value, isOr);
        }

        public ISqlCriteriaExpression StartsWith(string column, object value, bool isOr = false) 
        {
             return this.Filter(column, Operator.StartsWith, value, isOr);
        }

        public ISqlCriteriaExpression EndsWith(string column, object value, bool isOr = false) 
        {
             return this.Filter(column, Operator.EndsWith, value, isOr);
        }

        public ISqlCriteriaExpression GreaterThan(string column, object value, bool isOr = false) 
        {
            return this.Filter(column, Operator.GreaterThan, value, isOr);
        }

        public ISqlCriteriaExpression GreaterThan(string column, string sqlSubQuery, bool isOr = false, params object[] queryParams)
        {
            return this.Filter(column, Operator.GreaterThan, sqlSubQuery, isOr);
        }

        public ISqlCriteriaExpression GreaterThanEquals(string column, object value, bool isOr = false) 
        {
            return this.Filter(column, Operator.GreaterThanEqual, value, isOr);
        }

        public ISqlCriteriaExpression GreaterThanEquals(string column, string sqlSubQuery, bool isOr = false, params object[] queryParams)
        {
            return this.Filter(column, Operator.GreaterThanEqual, sqlSubQuery, isOr);
        }

        public ISqlCriteriaExpression LessThan(string column, object value, bool isOr = false) 
        {
            return this.Filter(column, Operator.LessThan, value, isOr);
        }

        public ISqlCriteriaExpression LessThan(string column, string sqlSubQuery, bool isOr = false, params object[] queryParams)
        {
            return this.Filter(column, Operator.LessThan, sqlSubQuery, isOr);
        }

        public ISqlCriteriaExpression LessThanEquals(string column, object value, bool isOr = false) 
        {
            return this.Filter(column, Operator.LessThanEqual, value, isOr);
        }

        public ISqlCriteriaExpression LessThanEquals(string column, string sqlSubQuery, bool isOr = false, params object[] queryParams)
        {
            return this.Filter(column, Operator.LessThanEqual, sqlSubQuery, isOr);
        }

        public ISqlCriteriaExpression In(string column, IEnumerable<object> values, bool isOr = false) 
        {
            if (values == null ||
                values.Count().Equals(0))
            {
                throw new ArgumentNullException("Values for In cannot be null or empty.");
            }

            List<object> valueList = values.ToList();
            StringBuilder inValueParamBuilder = new StringBuilder();

            for (int valueIndex = 0; valueIndex < valueList.Count; valueIndex++) 
            {
                string valueItem = valueList[valueIndex].ToString();

                inValueParamBuilder.Append(valueItem + (valueIndex < valueList.Count - 1 ? "," : string.Empty));
            }

            return this.Filter(column, Operator.In, inValueParamBuilder.ToString(), isOr);
        }

        public ISqlCriteriaExpression In(string column, string sqlSubQuery, bool isOr = false, params object[] queryParams)
        {
            return this.Filter(column, Operator.In, sqlSubQuery, isOr);
        }

        public ISqlCriteriaExpression NotIn(string column, IEnumerable<object> values, bool isOr = false) 
        {
            if (values == null ||
                values.Count().Equals(0))
            {
                throw new ArgumentNullException("Values for Not In cannot be null or empty.");
            }

            List<object> valueList = values.ToList();
            StringBuilder notInValueParamBuilder = new StringBuilder();

            for (int valueIndex = 0; valueIndex < valueList.Count; valueIndex++)
            {
                string valueItem = valueList[valueIndex].ToString();

                notInValueParamBuilder.Append(valueItem + (valueIndex < valueList.Count - 1 ? "," : string.Empty));
            }

            return this.Filter(column, Operator.NotIn, notInValueParamBuilder.ToString(), isOr);        
        }

        public ISqlCriteriaExpression NotIn(string column, string sqlSubQuery, bool isOr = false, params object[] queryParams)
        {
            return this.Filter(column, Operator.NotIn, sqlSubQuery, isOr);
        }

        public ISqlCriteriaExpression And(ISqlCriteria sqlCriteria)
        {
            this.globalSqlCriteria = new AndSqlCriteria(this.globalSqlCriteria, sqlCriteria);

            return this;
        }

        public ISqlCriteriaExpression And(string sqlFilter, params object[] filterParams)
        {
            TextSqlCriteria textSqlCriteria = new TextSqlCriteria(sqlFilter);

            this.globalSqlCriteria = new AndSqlCriteria(this.globalSqlCriteria, textSqlCriteria);

            return this;
        }

        public ISqlCriteriaExpression Or(ISqlCriteria sqlCriteria)
        {
            this.globalSqlCriteria = new OrSqlCriteria(this.globalSqlCriteria, sqlCriteria);

            return this;
        }

        public ISqlCriteriaExpression Or(string sqlFilter, params object[] filterParams) 
        {
            TextSqlCriteria textSqlCriteria = new TextSqlCriteria(sqlFilter);

            this.globalSqlCriteria = new OrSqlCriteria(this.globalSqlCriteria, textSqlCriteria);

            return this;
        }

        public ISqlCriteriaExpression Where(string sqlFilter, params object[] filterParams) 
        {
            this.globalSqlCriteria = new TextSqlCriteria(sqlFilter);

            return this;
        }

        public string GetSqlCriteria()
        {
            if (this.globalSqlCriteria == null)
            {
                return string.Empty;
            }

            return this.globalSqlCriteria.GetSqlCriteria();
        }

        public IDictionary<string, object> ParameterColumnValues 
        {
            get 
            {
                return this.parameterColumnValues;
            }
        }

        public SqlQueryDialectProviderBase DialectProvider
        {
            get 
            {
                return this.dialectProvider;
            }
        }
    }
}
