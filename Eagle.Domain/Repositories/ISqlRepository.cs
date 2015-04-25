using Eagle.Core.Query;
using Eagle.Core.SqlQueries;
using Eagle.Core.SqlQueries.Criterias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Domain.Repositories
{
    /// <summary>
    /// Sql Repository interface for an aggregate.
    /// </summary>
    public interface ISqlRepository<TAggregateRoot, TIdentityKey> where TAggregateRoot : class, IAggregateRoot<TIdentityKey>, new()
    {
        /// <summary>
        /// Return a repository context with Unit Of Work.
        /// </summary>
        IRepositoryContext RepositoryContext { get; }

        /// <summary>
        ///  Aadd an aggregate root to Repository.
        /// </summary>
        void Add(TAggregateRoot aggregateRoot);

        /// <summary>
        /// Update an aggregate root to Repository.
        /// </summary>
        void Update(TAggregateRoot aggregateRoot);

        /// <summary>
        /// Delete the specified aggregate root from Repository.
        /// </summary>
        void Delete(TAggregateRoot aggregateRoot);

        /// <summary>
        /// Delete an aggregate root from Repository by item key.
        /// </summary>
        void Delete(TIdentityKey id);

        /// <summary>
        /// Find the specific aggregate root by id or key.
        /// </summary>
        TAggregateRoot FindByKey(TIdentityKey id);

        /// <summary>
        /// Find the specific aggregate root by the specification from repository.
        /// </summary>
        TAggregateRoot Find(ISqlCriteriaExpression sqlCriteriaExpression);

        /// <summary>
        /// Find all of aggregate roots from repository.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TAggregateRoot> FindAll();

        /// <summary>
        /// Find all of aggregate roots from repository by the sql builder.
        /// </summary>
        /// <param name="sortPredicate"></param>
        /// <param name="sorOrder"></param>
        /// <returns></returns>
        IEnumerable<TAggregateRoot> FindAll(ISqlCriteriaExpression sqlCriteriaExpression);

        /// <summary>
        /// Find all of aggregate roots matching paging condition from repository by the sql builder.
        /// </summary>
        IPagingResult<TAggregateRoot> FindAll(ISqlCriteriaExpression sqlCriteriaExpression, 
                                              int pageNumber,
                                              int pageSize);

    }

    /// <summary>
    /// Sql Repository interface for an aggregate. The identity key is integer.
    /// </summary>
    public interface ISqlRepository<TAggregateRoot> : ISqlRepository<TAggregateRoot, int>
        where TAggregateRoot : class, IAggregateRoot<int>, IAggregateRoot, new()
    {

    }
}
