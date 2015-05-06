using Dappers;
using Eagle.Core;
using Eagle.Core.Query;
using Eagle.Core.SqlQueries;
using Eagle.Core.SqlQueries.Criterias;
using Eagle.Core.SqlQueries.DialectProvider;
using Eagle.Domain;
using Eagle.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Eagle.Repositories.Dapper
{
    public abstract class DapperSqlRepository<TAggregateRoot, TIdentityKey> : SqlRepository<TAggregateRoot, TIdentityKey> where TAggregateRoot : class, IAggregateRoot<TIdentityKey>, new()
    {
        private IDapperRepositoryContext dapperRepositoryContext;

        public DapperSqlRepository(IRepositoryContext repositoryContext)
            : base(repositoryContext)
        {
            if (repositoryContext is IDapperRepositoryContext)
            {
                this.dapperRepositoryContext = (IDapperRepositoryContext)repositoryContext;
            }
            else
            {
                throw new ArgumentException("The provided repository context type is invalid. DapperRepository requires an instance of DapperRepositoryContext to be initialized.");
            }
        }

        protected IDapperRepositoryContext DapperRepositoryContext
        {
            get
            {
                return this.dapperRepositoryContext;
            }
        }

        #region Aggregate root Creation/Update/Deletion using Dapper

        protected override void DoAdd(TAggregateRoot aggregateRoot)
        {
            string insertSqlStatement = this.GetAggregateRootInsertSqlStatement();

            object insertParameters = this.GetAggregateRootInsertParameters(aggregateRoot);

            this.DapperRepositoryContext.RegisterAdded(new CommandSqlParameters { CommandSql = insertSqlStatement, Parameters = insertParameters, CommandType = CommandType.Text });
        }

        protected override void DoUpdate(TAggregateRoot aggregateRoot)
        {
            string updateSqlStatement = this.GetAggregateRootUpdateSqlStatement();

            object updateParameters = this.GetAggregateRootUpdateParameters(aggregateRoot);

            this.DapperRepositoryContext.RegisterModified(new CommandSqlParameters { CommandSql = updateSqlStatement, Parameters = updateParameters, CommandType = CommandType.Text });
        }

        protected override void DoDelete(TAggregateRoot aggregateRoot)
        {
            string deleteSqlStatement = this.GetAggregateRootDeleteSqlStatement();

            object deleteParameters = this.GetAggregateRootDeleteParameters(aggregateRoot);

            this.DapperRepositoryContext.RegisterDeleted(new CommandSqlParameters { CommandSql = deleteSqlStatement, Parameters = deleteParameters, CommandType = CommandType.Text });
        }

        protected override void DoDelete(TIdentityKey id)
        {
            throw new NotImplementedException();
        }

        #endregion

        protected override TAggregateRoot DoFindByKey(TIdentityKey id)
        {
            using (IDbConnection dbConnection = this.DapperRepositoryContext.CreateConnection())
            {
                ISqlCriteriaExpression sqlCriteriaExpression = SqlQueryDialectProviderFactory.Default.SqlCriteriaExpression();
                sqlCriteriaExpression.Equals("Id", id);

                string sqlStatement = this.GetSingleAggregateRootQuerySqlStatementByCriteria(sqlCriteriaExpression);
                object parameters = this.GetSingleAggregateRootQueryParametersByCriteria(sqlCriteriaExpression);

                return dbConnection.Query<TAggregateRoot>(sqlStatement, parameters, null, true, null, CommandType.Text).SingleOrDefault();
            }
        }

        protected override TAggregateRoot DoFind(ISqlCriteriaExpression sqlCriteriaExpression)
        {
            using (IDbConnection dbConnection = this.DapperRepositoryContext.CreateConnection())
            {
                string sqlStatement = this.GetSingleAggregateRootQuerySqlStatementByCriteria(sqlCriteriaExpression);
                object parameters = this.GetSingleAggregateRootQueryParametersByCriteria(sqlCriteriaExpression);

                return dbConnection.Query<TAggregateRoot>(sqlStatement, parameters, null, true, null, CommandType.Text).SingleOrDefault();
            }
        }

        protected override IEnumerable<TAggregateRoot> DoFindAll()
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<TAggregateRoot> DoFindAll(ISqlCriteriaExpression sqlCriteriaExpression)
        {
            using (IDbConnection dbConnection = this.DapperRepositoryContext.CreateConnection())
            {
                string sqlStatement = this.GetAggregateRootListQuerySqlStatementByCriteria(sqlCriteriaExpression);
                object parameters = this.GetAggregateRootListQueryParametersByCriteria(sqlCriteriaExpression);

                return dbConnection.Query<TAggregateRoot>(sqlStatement, parameters, null, true, null, CommandType.Text);
            }
        }

        protected override IPagingResult<TAggregateRoot> DoFindAll(ISqlCriteriaExpression sqlCriteriaExpression, int pageNumber, int pageSize)
        {
            using (IDbConnection dbConnection = this.DapperRepositoryContext.CreateConnection())
            {
                string pagingSqlStatement = this.GetAggregateRootListPagingQuerySqlStatementByCriteria(sqlCriteriaExpression, pageNumber, pageSize);

                object parameters = this.GetAggregateRootListPagingQueryParametersByCriteria(sqlCriteriaExpression);

                IEnumerable<TAggregateRoot> pagedAggregateRoots =
                    dbConnection.Query<TAggregateRoot>(pagingSqlStatement, parameters, null, true, null, CommandType.Text);

                int totalRecords = pagedAggregateRoots.Count();

                int totalPages = (totalRecords + pageSize - 1) / pageSize;

                return new PagingResult<TAggregateRoot>(totalRecords,
                                                        totalPages,
                                                        pageNumber,
                                                        pageSize,
                                                        pagedAggregateRoots.Select(aggregateRoot => aggregateRoot).ToList());
            }
        }

        protected abstract string GetSingleAggregateRootQuerySqlStatementByCriteria(ISqlCriteriaExpression sqlCriteriaExpression);

        protected abstract object GetSingleAggregateRootQueryParametersByCriteria(ISqlCriteriaExpression sqlCriteriaExpression);

        protected abstract string GetAggregateRootListQuerySqlStatementByCriteria(ISqlCriteriaExpression sqlCriteriaExpression);

        protected abstract object GetAggregateRootListQueryParametersByCriteria(ISqlCriteriaExpression sqlCriteriaExpression);

        protected abstract string GetAggregateRootListPagingQuerySqlStatementByCriteria(ISqlCriteriaExpression sqlCriteriaExpression, int pageNumber, int pageSize);

        protected abstract object GetAggregateRootListPagingQueryParametersByCriteria(ISqlCriteriaExpression sqlCriteriaExpression);

        protected abstract string GetAggregateRootInsertSqlStatement();

        protected abstract object GetAggregateRootInsertParameters(TAggregateRoot aggregateRoot);

        protected abstract string GetAggregateRootUpdateSqlStatement();

        protected abstract object GetAggregateRootUpdateParameters(TAggregateRoot aggregateRoot);

        protected abstract string GetAggregateRootDeleteSqlStatement();

        protected abstract object GetAggregateRootDeleteParameters(TAggregateRoot aggregateRoot);

    }

}
