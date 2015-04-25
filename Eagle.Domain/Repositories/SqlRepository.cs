using Eagle.Core.Query;
using Eagle.Core.SqlQueries;
using Eagle.Core.SqlQueries.Criterias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Domain.Repositories
{
    public abstract class SqlRepository<TAggregateRoot, TIdentityKey> : ISqlRepository<TAggregateRoot, TIdentityKey>
        where TAggregateRoot : class, IAggregateRoot<TIdentityKey>, new()
    {

        private IRepositoryContext repositoryContext;

        public SqlRepository(IRepositoryContext repositoryContext)
        {
            this.repositoryContext = repositoryContext;
        }

        public IRepositoryContext RepositoryContext
        {
            get
            {
                return this.repositoryContext;
            }
        }

        #region Aggregate root Creation/Update/Deletion

        public void Add(TAggregateRoot aggregateRoot)
        {
            this.DoAdd(aggregateRoot);
        }

        public void Update(TAggregateRoot aggregateRoot)
        {
            this.DoUpdate(aggregateRoot);
        }

        public void Delete(TAggregateRoot aggregateRoot)
        {
            this.DoDelete(aggregateRoot);
        }

        public void Delete(TIdentityKey id)
        {
            this.DoDelete(id);
        }

        #endregion

        #region Find the aggregate root by Id or specification

        public TAggregateRoot FindByKey(TIdentityKey id)
        {
            return this.DoFindByKey(id);
        }

        public TAggregateRoot Find(ISqlCriteriaExpression sqlCriteriaExpression)
        {
            return this.DoFind(sqlCriteriaExpression);
        }

        #endregion

        #region Aggregate roots queries

        public IEnumerable<TAggregateRoot> FindAll()
        {
            return this.DoFindAll();
        }

        public IEnumerable<TAggregateRoot> FindAll(ISqlCriteriaExpression sqlCriteriaExpression)
        {
            return this.DoFindAll(sqlCriteriaExpression);
        }

        public IPagingResult<TAggregateRoot> FindAll(ISqlCriteriaExpression sqlCriteriaExpression, int pageNumber, int pageSize)
        {
            return this.DoFindAll(sqlCriteriaExpression, pageNumber, pageSize);
        }

        #endregion

        #region Protected methods

        #region Aggregate root Creation/Update/Deletion

        protected abstract void DoAdd(TAggregateRoot aggregateRoot);

        protected abstract void DoUpdate(TAggregateRoot aggregateRoot);

        protected abstract void DoDelete(TAggregateRoot aggregateRoot);

        protected abstract void DoDelete(TIdentityKey id);

        #endregion

        #region Find the aggregate root by Id or specification

        protected abstract TAggregateRoot DoFindByKey(TIdentityKey id);

        protected abstract TAggregateRoot DoFind(ISqlCriteriaExpression sqlCriteriaExpression);

        #endregion

        #region Aggregate roots Queries

        protected abstract IEnumerable<TAggregateRoot> DoFindAll();

        protected abstract IEnumerable<TAggregateRoot> DoFindAll(ISqlCriteriaExpression sqlCriteriaExpression);

        protected abstract IPagingResult<TAggregateRoot> DoFindAll(ISqlCriteriaExpression sqlCriteriaExpression,
                                                                   int pageNumber,
                                                                   int pageSize);

        #endregion

        #endregion
    }

    public abstract class SqlRepository<TAggregateRoot> : SqlRepository<TAggregateRoot, int>, ISqlRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot<int>, IAggregateRoot, new()
    {
        public SqlRepository(IRepositoryContext repositoryContext) : base(repositoryContext) { }
    }

}
