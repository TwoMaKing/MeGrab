using Eagle.Core.Exceptions;
using Eagle.Core.Extensions;
using Eagle.Core.Query;
using Eagle.Core.SqlQueries;
using Eagle.Core.SqlQueries.Criterias;
using Eagle.Core.SqlQueries.DialectProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Core.SqlQueries
{
    public class SqlBuilder : ISqlBuilder
    {
        private ISqlQueryDialectProvider dialectProvider;

        private string fromTable = string.Empty;

        private List<string> innerJoinTables = new List<string>();

        private List<string> leftOuterJoinTables = new List<string>();

        private List<string> rightOuterJoinTables = new List<string>();

        private StringBuilder querySqlBuilder = new StringBuilder();

        private List<string> selectColumns = new List<string>();

        private string selectDistinctSql = string.Empty;

        private StringBuilder selectFunctionBuilder = new StringBuilder();

        private List<string> orderByColumns = new List<string>();

        private string groupBySql = string.Empty;

        private string identityColumn = string.Empty;

        private int pageNumber = 0;

        private int pageSize = 0;

        private bool usingPaging;

        private IDictionary<string, object> parameterColumnValues = new Dictionary<string, object>();

        private IList<object> parameterValues = new List<object>();

        public SqlBuilder() : this(SqlQueryDialectProviderFactory.Default) { }

        public SqlBuilder(SqlQueryDialectProviderBase dbProvider) 
        {
            this.dialectProvider = dbProvider;
        }

        public static ISqlBuilder Create() 
        {
            return new SqlBuilder(SqlQueryDialectProviderFactory.Default);
        }

        protected ISqlCriteria SqlCriteria
        {
            get;
            set;
        }

        public ISqlQueryDialectProvider DialectProvider
        {
            get 
            {
                return this.dialectProvider;
            }
        }

        public ISqlBuilder From(string table)
        {
            if (string.IsNullOrEmpty(table) ||
                string.IsNullOrWhiteSpace(table))
            {
                throw new ArgumentNullException("The table cannot be null or empty.");
            }

            if (!string.IsNullOrEmpty(this.fromTable) &&
                !string.IsNullOrWhiteSpace(this.fromTable))
            {
                throw new InfrastructureException("From has already been specified.");
            }

            this.fromTable = table;

            return this;
        }

        public ISqlBuilder InnerJoin(string joinTable, string fromKey, string joinKey)
        {
            StringBuilder innerJoinTableBuilder = new StringBuilder();
            innerJoinTableBuilder.Append(" INNER JOIN ");
            innerJoinTableBuilder.Append(joinTable);
            innerJoinTableBuilder.Append(" ON ");
            innerJoinTableBuilder.Append(this.fromTable);
            innerJoinTableBuilder.Append(".");
            innerJoinTableBuilder.Append(fromKey);
            innerJoinTableBuilder.Append(" = ");
            innerJoinTableBuilder.Append(joinTable);
            innerJoinTableBuilder.Append(".");
            innerJoinTableBuilder.Append(joinKey);

            this.innerJoinTables.Add(innerJoinTableBuilder.ToString());

            return this;
        }

        public ISqlBuilder LeftOuterJoin(string joinTable, string fromKey, string joinKey)
        {
            StringBuilder leftOuterJoinTableBuilder = new StringBuilder();
            leftOuterJoinTableBuilder.Append(" LEFT OUTER JOIN ");
            leftOuterJoinTableBuilder.Append(joinTable);
            leftOuterJoinTableBuilder.Append(" ON ");
            leftOuterJoinTableBuilder.Append(this.fromTable);
            leftOuterJoinTableBuilder.Append(".");
            leftOuterJoinTableBuilder.Append(fromKey);
            leftOuterJoinTableBuilder.Append(" = ");
            leftOuterJoinTableBuilder.Append(joinTable);
            leftOuterJoinTableBuilder.Append(".");
            leftOuterJoinTableBuilder.Append(joinKey);

            this.leftOuterJoinTables.Add(leftOuterJoinTableBuilder.ToString());

            return this;
        }

        public ISqlBuilder RightOuterJoin(string joinTable, string fromKey, string joinKey)
        {
            StringBuilder rightOuterJoinTableBuilder = new StringBuilder();
            rightOuterJoinTableBuilder.Append(" RIGHT OUTER JOIN ");
            rightOuterJoinTableBuilder.Append(joinTable);
            rightOuterJoinTableBuilder.Append(" ON ");
            rightOuterJoinTableBuilder.Append(this.fromTable);
            rightOuterJoinTableBuilder.Append(".");
            rightOuterJoinTableBuilder.Append(fromKey);
            rightOuterJoinTableBuilder.Append(" = ");
            rightOuterJoinTableBuilder.Append(joinTable);
            rightOuterJoinTableBuilder.Append(".");
            rightOuterJoinTableBuilder.Append(joinKey);

            this.rightOuterJoinTables.Add(rightOuterJoinTableBuilder.ToString());

            return this;
        }

        public ISqlBuilder And(string criteria)
        {
            return this.And(new TextSqlCriteria(criteria));
        }

        public ISqlBuilder And(ISqlCriteria criteria)
        {
            if (this.SqlCriteria == null)
            {
                this.SqlCriteria = new TextSqlCriteria(string.Empty);
            }

            this.SqlCriteria = new AndSqlCriteria(this.SqlCriteria, criteria);

            return this;
        }

        public ISqlBuilder Or(string criteria)
        {
            return this.Or(new TextSqlCriteria(criteria));
        }

        public ISqlBuilder Or(ISqlCriteria criteria)
        {
            if (this.SqlCriteria == null)
            {
                this.SqlCriteria = new TextSqlCriteria(string.Empty);
            }
            
            this.SqlCriteria = new OrSqlCriteria(this.SqlCriteria, criteria);

            return this;
        }

        public ISqlBuilder Filter(string column, Operator @operator, object value, bool isOr = false)
        {
            OperatorSqlCriteria sqlCriteria = OperatorSqlCriteria.Create(this.DialectProvider, column, @operator);
            
            if (isOr)
            {
                this.Or(sqlCriteria);
            }
            else
            {
                this.And(sqlCriteria);
            }

            this.parameterColumnValues.Add(sqlCriteria.ParameterColumnName, value);

            this.parameterValues.Add(value);

            return this;
        }

        public ISqlBuilder Equals(string column, object value, bool isOr = false)
        {
            return this.Filter(column, Operator.Equal, value, isOr);
        }

        public ISqlBuilder NotEquals(string column, object value, bool isOr = false)
        {
            return this.Filter(column, Operator.NotEqual, value, isOr);
        }

        public ISqlBuilder Contains(string column, object value, bool isOr = false)
        {
            return this.Filter(column, Operator.Contains, "%" + value + "%", isOr);
        }

        public ISqlBuilder StartsWith(string column, object value, bool isOr = false)
        {
            return this.Filter(column, Operator.StartsWith, "%" + value, isOr);
        }

        public ISqlBuilder EndsWith(string column, object value, bool isOr = false)
        {
            return this.Filter(column, Operator.EndsWith, value + "%", isOr);
        }

        public ISqlBuilder GreaterThan(string column, object value, bool isOr = false)
        {
            return this.Filter(column, Operator.GreaterThan, value, isOr);
        }

        public ISqlBuilder GreaterThanEquals(string column, object value, bool isOr = false)
        {
            return this.Filter(column, Operator.GreaterThanEqual, value, isOr);
        }

        public ISqlBuilder LessThan(string column, object value, bool isOr = false)
        {
            return this.Filter(column, Operator.LessThan, value, isOr);
        }

        public ISqlBuilder LessThanEquals(string column, object value, bool isOr = false)
        {
            return this.Filter(column, Operator.LessThanEqual, value, isOr);
        }

        public ISqlBuilder In(string column, IEnumerable<object> values, bool isOr = false)
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

        public ISqlBuilder NotIn(string column, IEnumerable<object> values, bool isOr = false)
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

        public ISqlBuilder OrderBy(string column, SortOrder sortOrder)
        {
            string orderByItem = column + (sortOrder == SortOrder.Ascending ? " ASC " : " DESC ");

            this.orderByColumns.Add(orderByItem);

            return this;
        }

        public ISqlBuilder GroupBy(string[] columns, string having = "")
        {
            if (!string.IsNullOrEmpty(this.groupBySql) &&
                !string.IsNullOrWhiteSpace(this.groupBySql))
            {
                throw new InfrastructureException("Group By has already been specified.");
            }

            if (columns == null ||
                columns.Length.Equals(0))
            {
                return this;
            }

            this.groupBySql = string.Join(",", columns) + 
                             (string.IsNullOrEmpty(having) ? 
                              string.Empty :
                              " HAVING " + having);

            return this;
        }

        public ISqlBuilder Distinct()
        {
            if (!string.IsNullOrEmpty(this.selectDistinctSql) &&
                !string.IsNullOrWhiteSpace(this.selectDistinctSql))
            {
                throw new InfrastructureException("Distinct has already been specified.");
            }

            this.selectDistinctSql = " DISTINCT ";

            return this;
        }

        public ISqlBuilder Count(string column)
        {
            this.selectFunctionBuilder.Append(" COUNT (" + column + "), ");

            return this;
        }

        public ISqlBuilder Max(string column)
        {
            this.selectFunctionBuilder.Append(" MAX (" + column + "), ");

            return this;
        }

        public ISqlBuilder Min(string column)
        {
            this.selectFunctionBuilder.Append(" MIN (" + column + "), ");

            return this;
        }

        public ISqlBuilder Sum(string column)
        {
            this.selectFunctionBuilder.Append(" SUM (" + column + "), ");

            return this;
        }

        public ISqlBuilder Select(params string[] columns)
        {
            if (columns == null)
            {
                if (!this.selectColumns.Contains("*"))
                {
                    this.selectColumns.Add("*");
                }
            }
            else
            {
                this.selectColumns.AddRange(columns);
            }

            return this;
        }

        public ISqlBuilder Where(string wherePredicate, IEnumerable<object> paramValues)
        {
            this.SqlCriteria = new TextSqlCriteria(wherePredicate);

            return this;
        }

        public ISqlBuilder Where(ISqlCriteriaExpression wherePredicate)
        {
            if (wherePredicate != null)
            {
                this.Where(wherePredicate.GetSqlCriteria(),
                           wherePredicate.ParameterColumnValues.Values);
            }

            return this;
        }

        public ISqlBuilder Page(string identityColumn, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
            {
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "The pageNumber is one-based and should be larger than zero.");
            }

            if (pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "The pageSize is one-based and should be larger than zero.");
            }

            this.identityColumn = identityColumn;
            this.pageNumber = pageNumber;
            this.pageSize = pageSize;
            this.usingPaging = true;

            return this;
        }

        public ISqlBuilder Clear()
        {
            this.querySqlBuilder.Clear();
            this.selectFunctionBuilder.Clear();

            this.innerJoinTables.Clear();
            this.leftOuterJoinTables.Clear();
            this.rightOuterJoinTables.Clear();
            this.selectColumns.Clear();
            this.orderByColumns.Clear();
            this.parameterValues.Clear();
            this.parameterColumnValues.Clear();

            this.fromTable = string.Empty;
            this.selectDistinctSql = string.Empty;
            this.groupBySql = string.Empty;
            this.identityColumn = string.Empty;
            this.pageNumber = 0;
            this.pageSize = 0;
            this.usingPaging = false;

            ParameterColumnCache.Instance.Reset();

            return this;
        }

        public string GetTables()
        {
            StringBuilder tableBuilder = new StringBuilder();

            tableBuilder.Append(this.fromTable);

            if (this.innerJoinTables != null &&
                this.innerJoinTables.Count > 0)
            {
                this.innerJoinTables.ForEach((joinSql) => tableBuilder.Append(joinSql));
            }

            if (this.leftOuterJoinTables != null &&
                this.leftOuterJoinTables.Count > 0)
            {
                this.leftOuterJoinTables.ForEach((joinSql) => tableBuilder.Append(joinSql));
            }

            if (this.rightOuterJoinTables != null &&
                this.rightOuterJoinTables.Count > 0)
            {
                this.rightOuterJoinTables.ForEach((joinSql) => tableBuilder.Append(joinSql));
            }

            return tableBuilder.ToString();
        }

        public string[] GetColumns()
        {
            return this.selectColumns.ToArray();
        }

        public string GetOrderBy()
        {
            if (this.orderByColumns == null ||
                this.orderByColumns.Count.Equals(0))
            {
                return string.Empty;
            }

            return string.Join(",", this.orderByColumns.ToArray());
        }

        public string GetGroupBy()
        {
            return this.groupBySql;
        }

        public string GetPredicate()
        {
            if (this.SqlCriteria == null)
            {
                return string.Empty;
            }

            return this.SqlCriteria.GetSqlCriteria();
        }

        public string GetQuerySql()
        {
            string tables = this.GetTables();
            string sqlPredicate = this.GetPredicate();
            string orderBySql = this.GetOrderBy();

            StringBuilder includedColumnSqlBuilder = new StringBuilder();
            includedColumnSqlBuilder.Append(this.selectDistinctSql)
                                    .Append(this.selectFunctionBuilder.ToString().TrimEnd(new char[] { ',', ' ' }))
                                    .Append(this.selectColumns.Count.Equals(0) ? "*" : string.Join(",", this.selectColumns));
            string includedColumnSql = includedColumnSqlBuilder.ToString();

            if (this.usingPaging)
            {
                int topCount = this.pageSize;
                int skipCount = (this.pageNumber - 1) * this.pageSize;

                return this.DialectProvider.CreateSelectRangeStatement(tables,
                                                                       sqlPredicate, 
                                                                       orderBySql, 
                                                                       topCount, 
                                                                       skipCount, 
                                                                       this.identityColumn, 
                                                                       true, 
                                                                       this.groupBySql, 
                                                                       includedColumnSql);
            }

            this.querySqlBuilder = new StringBuilder();

            this.querySqlBuilder.Append("SELECT ")
                                .Append(includedColumnSql)
                                .Append(" FROM ")
                                .Append(tables);

            if (sqlPredicate.HasValue())
            {
                this.querySqlBuilder.Append(" WHERE ");
                this.querySqlBuilder.Append(sqlPredicate);
            }

            if (this.groupBySql.HasValue())
            {
                this.querySqlBuilder.Append(" GROUP BY ");
                this.querySqlBuilder.Append(this.groupBySql);
            }
       
            if (orderBySql.HasValue())
            {
                this.querySqlBuilder.Append(" ORDER BY ");
                this.querySqlBuilder.Append(orderBySql); 
            }

            return this.querySqlBuilder.ToString();
        }

        public IDictionary<string, object> GetParameters()
        {
            return this.parameterColumnValues;
        }
    }
}
