using Eagle.Core.Exceptions;
using Eagle.Core.Extensions;
using Eagle.Core.Query;
using Eagle.Core.SqlQueries.DialectProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Core.SqlQueries.Criterias
{
    public abstract class OperatorSqlCriteria : ISqlCriteria
    {
        protected OperatorSqlCriteria() { }

        protected OperatorSqlCriteria(ISqlQueryDialectProvider dialectProvider, string columnName)
        {
            this.DialectProvider = dialectProvider;
            this.DbColumnName = columnName;
            this.BuildedDbColumnName = this.DialectProvider.BuildColumnName(this.DbColumnName).Trim();
            string tempParameterColumn = ParameterColumnCache.Instance.GetParameterColumn(columnName);
            this.ParameterColumnName = this.DialectProvider.BuildParameterName(tempParameterColumn).Trim();
        }

        protected OperatorSqlCriteria(ISqlQueryDialectProvider dialectProvider, string columnName, string sqlSubQuery)
        {
            this.DialectProvider = dialectProvider;
            this.DbColumnName = columnName;
            this.BuildedDbColumnName = this.DialectProvider.BuildColumnName(this.DbColumnName).Trim();
            
            this.SqlSubQuery = sqlSubQuery;
        }

        public ISqlQueryDialectProvider DialectProvider 
        {
            get;
            set;
        }

        public string DbColumnName 
        { 
            get; 
            set; 
        }

        public string BuildedDbColumnName
        {
            get;
            set;
        }

        public string ParameterColumnName
        {
            get;
            set;
        }

        public string SqlSubQuery 
        { 
            get; 
            set; 
        }

        public static OperatorSqlCriteria Create(ISqlQueryDialectProvider dialectProvider, string dbColumn, Operator @operator) 
        {
            switch (@operator)
            { 
                case Operator.Equal:
                    return new EqualSqlCriteria(dialectProvider, dbColumn);
                case Operator.NotEqual:
                    return new NotEqualSqlCriteria(dialectProvider, dbColumn);
                case Operator.GreaterThan:
                    return new GreaterThanSqlCriteria(dialectProvider, dbColumn);
                case Operator.GreaterThanEqual:
                    return new GreaterThanSqlCriteria(dialectProvider, dbColumn);
                case Operator.LessThan:
                    return new LessThanSqlCriteria(dialectProvider, dbColumn);
                case Operator.LessThanEqual:
                    return new LessThanEqualSqlCriteria(dialectProvider, dbColumn);
                case Operator.Contains:
                case Operator.StartsWith:
                case Operator.EndsWith:
                    return new LikeSqlCriteria(dialectProvider, dbColumn);
                case Operator.In:
                    return new InSqlCriteria(dialectProvider, dbColumn);
                case Operator.NotIn:
                    return new NotInSqlCriteria(dialectProvider, dbColumn);
                default:
                    return null;
            }
        }

        public static OperatorSqlCriteria Create(ISqlQueryDialectProvider dialectProvider, string dbColumn, Operator @operator, string sqlSubQuery)
        {
            switch (@operator)
            {
                case Operator.GreaterThan:
                    return new GreaterThanSqlCriteria(dialectProvider, dbColumn);
                case Operator.GreaterThanEqual:
                    return new GreaterThanSqlCriteria(dialectProvider, dbColumn);
                case Operator.LessThan:
                    return new LessThanSqlCriteria(dialectProvider, dbColumn);
                case Operator.LessThanEqual:
                    return new LessThanEqualSqlCriteria(dialectProvider, dbColumn);
                case Operator.In:
                    return new InSqlCriteria(dialectProvider, dbColumn);
                case Operator.NotIn:
                    return new NotInSqlCriteria(dialectProvider, dbColumn);
                default:
                    throw new InfrastructureException("This query operator doesn't support the sub query sql.");
            }
        }

        public virtual string GetSqlCriteria() 
        {
            if (!this.SqlSubQuery.HasValue())
            {
                return string.Format(@" {0} {1} {2} ", this.BuildedDbColumnName,
                                                       this.GetOperatorChar(),
                                                       this.ParameterColumnName);
            }
            else
            {
                return string.Format(@" {0} {1} {2} ", this.BuildedDbColumnName,
                                                       this.GetOperatorChar(),
                                                       " (" + this.SqlSubQuery + ") ");
            }
        }

        protected abstract string GetOperatorChar(); 
    }
}
