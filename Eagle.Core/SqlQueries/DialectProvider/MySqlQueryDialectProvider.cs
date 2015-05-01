using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Eagle.Core.SqlQueries.DialectProvider
{
    public class MySqlQueryDialectProvider : SqlQueryDialectProviderBase
    {
        private const char Parameter_Prefix = '?';

        public override string SelectLastInsertedRowAutoIDStatement
        {
            get 
            { 
                return "SELECT LAST_INSERT_ID()"; 
            }
        }

        public override char ParameterPrefix
        {
            get 
            {
                return Parameter_Prefix; 
            }
        }

        public override char ParameterLeftToken
        {
            get 
            { 
                return '`'; 
            }
        }

        public override char ParameterRightToken
        {
            get 
            { 
                return '`'; 
            }
        }

        public override char WildCharToken
        {
            get 
            { 
                return '%'; 
            }
        }

        public override char WildSingleCharToken
        {
            get 
            {
                return '_'; 
            }
        }

        public override string CreateSelectRangeStatement(string tableName, 
                                                          string where, 
                                                          string orderBy, 
                                                          int topCount, 
                                                          int skipCount,
                                                          string identityColumn, 
                                                          bool identityColumnIsNumberOrSequence = true,
                                                          string groupBy = null, 
                                                          params string[] includedColumns)
        {

            if (includedColumns == null)
            {
                includedColumns = new string[] { "*" };
            }

            if (identityColumnIsNumberOrSequence &&
                SqlQueryUtils.OrderByStartsWith(orderBy, identityColumn) &&
                (string.IsNullOrEmpty(groupBy) ||
                groupBy.Equals(identityColumn, StringComparison.InvariantCultureIgnoreCase)))
            {
                return CreateSelectRangeStatementForSortedRows(tableName, where, includedColumns, orderBy, groupBy,
                                                               topCount, skipCount, identityColumn, 
                                                               SqlQueryUtils.OrderByStartsWith(orderBy, identityColumn + " DESC"));
            }
            else
            {
                return CreateSelectRangeStatementForUnsortedRows(tableName, where, includedColumns, orderBy, groupBy, 
                                                                 topCount, skipCount, identityColumn);
            }
        }

         private string CreateSelectRangeStatementForSortedRows(string tableName, 
                                                                string where, 
                                                                string[] columns, 
                                                                string orderBy, 
                                                                string groupBy, 
                                                                int topCount, 
                                                                int skipCount, 
                                                                string identityColumn, 
                                                                bool isIdentityColumnDesc)
        {
            //SELECT ID
            //FROM TABLE 
            //WHERE  ID >= (SELECT ID FROM TABLE ORDER BY ID LIMIT 90000,1) LIMIT 10;

            StringBuilder outerSqlBuilder = new StringBuilder("SELECT ");

            for (int i = 0; i < columns.Length; ++i)
            {
                SqlQueryUtils.AppendColumnName(outerSqlBuilder, columns[i], this.ParameterLeftToken, this.ParameterRightToken);

                if (i < columns.Length - 1)
                {
                    outerSqlBuilder.Append(',');
                }
            }

            outerSqlBuilder.Append(" FROM ");
            outerSqlBuilder.Append(tableName);
            outerSqlBuilder.Append(" WHERE ");

            StringBuilder innerWhereClipBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(where) &&
                !string.IsNullOrWhiteSpace(where))
            {
                innerWhereClipBuilder.Append(" WHERE " + where);
            }

            StringBuilder orderByGroupByBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(orderBy) &&
                !string.IsNullOrWhiteSpace(orderBy))
            {
                orderByGroupByBuilder.Append(" ORDER BY " + orderBy);
            }

            if (!string.IsNullOrEmpty(groupBy) &&
                !string.IsNullOrWhiteSpace(groupBy))
            {
                orderByGroupByBuilder.Append(" GROUP BY " + groupBy);
            }

            innerWhereClipBuilder.Append(orderByGroupByBuilder.ToString());

            StringBuilder innerSqlBuilder = new StringBuilder();
            innerSqlBuilder.Append(identityColumn);
            innerSqlBuilder.Append(isIdentityColumnDesc ? " <= " : " >= ");
            innerSqlBuilder.Append('(');
            innerSqlBuilder.Append("SELECT ");
            innerSqlBuilder.Append(identityColumn);
            innerSqlBuilder.Append(" FROM ");
            innerSqlBuilder.Append(tableName);
            innerSqlBuilder.Append(innerWhereClipBuilder.ToString());
            innerSqlBuilder.Append(" LIMIT ");
            innerSqlBuilder.Append(skipCount == 0 ? "1" : skipCount.ToString() + ", 1");
            innerSqlBuilder.Append(')');
            innerSqlBuilder.Append("LIMIT ");
            innerSqlBuilder.Append(topCount);

            string outerWhereSql = string.Empty;

            if (where.Length == 0)
            {
                outerWhereSql = innerSqlBuilder.ToString();
            }
            else
            {
                outerWhereSql = "(" + where + ") AND " + innerSqlBuilder.ToString();
            }

            outerSqlBuilder.Append(outerWhereSql);

            outerSqlBuilder.Append(orderByGroupByBuilder.ToString());



            return SqlQueryUtils.ReplaceDatabaseTokens(outerSqlBuilder.ToString(), this.ParameterRightToken, this.ParameterRightToken, this.ParameterPrefix, this.WildCharToken, this.WildSingleCharToken);
        }

        protected string CreateSelectRangeStatementForUnsortedRows(string tableName, 
                                                                   string where, 
                                                                   string[] columns, 
                                                                   string orderBy, 
                                                                   string groupBy, 
                                                                   int topCount, 
                                                                   int skipCount, 
                                                                   string identyColumn)
        {
            //SELECT * FROM TESTTABLE WHERE ID IN 
            //(SELECT ID FROM (SELECT ID FROM TESTTABLE ORDER BY ID DESC LIMIT 10000, 10) AS TEMP)

            StringBuilder outerSqlBuilder = new StringBuilder("SELECT ");

            for (int i = 0; i < columns.Length; ++i)
            {
                SqlQueryUtils.AppendColumnName(outerSqlBuilder, columns[i], this.ParameterLeftToken, this.ParameterRightToken);

                if (i < columns.Length - 1)
                {
                    outerSqlBuilder.Append(',');
                }
            }

            outerSqlBuilder.Append(" FROM ");
            outerSqlBuilder.Append(tableName);
            outerSqlBuilder.Append(" WHERE ");
            outerSqlBuilder.Append(identyColumn);
            outerSqlBuilder.Append(" IN ");

            outerSqlBuilder.Append("(");
            outerSqlBuilder.Append(" SELECT ");
            outerSqlBuilder.Append(identyColumn);
            outerSqlBuilder.Append(" FROM ");

            StringBuilder innerWhereClipBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(where) &&
                !string.IsNullOrWhiteSpace(where))
            {
                innerWhereClipBuilder.Append(" WHERE " + where);
            }

            StringBuilder orderByGroupByBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(orderBy) &&
                !string.IsNullOrWhiteSpace(orderBy))
            {
                orderByGroupByBuilder.Append(" ORDER BY " + orderBy);
            }

            if (!string.IsNullOrEmpty(groupBy) &&
                !string.IsNullOrWhiteSpace(groupBy))
            {
                orderByGroupByBuilder.Append(" GROUP BY " + groupBy);
            }

            innerWhereClipBuilder.Append(orderByGroupByBuilder.ToString());

            StringBuilder innerSqlBuilder = new StringBuilder();
            innerSqlBuilder.Append("(");
            innerSqlBuilder.Append("SELECT ");
            innerSqlBuilder.Append(identyColumn);
            innerSqlBuilder.Append(" FROM ");
            innerSqlBuilder.Append(tableName);
            innerSqlBuilder.Append(innerWhereClipBuilder.ToString());
            innerSqlBuilder.Append(" LIMIT ");
            innerSqlBuilder.Append(skipCount == 0 ? topCount.ToString() : skipCount.ToString() + ", " + topCount.ToString());
            innerSqlBuilder.Append(") ");
            innerSqlBuilder.Append("AS ");
            innerSqlBuilder.Append("TEMP");
            innerSqlBuilder.Append(")");

            outerSqlBuilder.Append(innerSqlBuilder.ToString());

            return SqlQueryUtils.ReplaceDatabaseTokens(outerSqlBuilder.ToString(), this.ParameterRightToken, this.ParameterRightToken, this.ParameterPrefix, this.WildCharToken, this.WildSingleCharToken);
        }

    }
}
