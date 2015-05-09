using Dappers;
using Eagle.Core.Query;
using Eagle.Core.SqlQueries;
using Eagle.Core.SqlQueries.Criterias;
using Eagle.Core.SqlQueries.DialectProvider;
using Eagle.Domain.Repositories;
using Eagle.Repositories.Dapper;
using MeGrab.Domain.Models;
using MeGrab.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MeGrab.Domain.Repositories.Sql
{
    public class MeGrabUserRepository : DapperSqlRepository<MeGrabUser>, IMeGrabUserSqlRepository
    {
        public MeGrabUserRepository(IRepositoryContext repositoryContext)
            : base(repositoryContext)
        { }

        protected override MeGrabUser DoFind(ISqlCriteriaExpression sqlCriteriaExpression)
        {
            using (IDbConnection dbConnection = this.DapperRepositoryContext.CreateConnection())
            {
                string sqlStatement = this.GetSingleAggregateRootQuerySqlStatementByCriteria(sqlCriteriaExpression);
                object parameters = this.GetSingleAggregateRootQueryParametersByCriteria(sqlCriteriaExpression);

                Func<MeGrabUser, MeGrabMembership, MeGrabUser> userMap = (u, m) =>
                {
                    u.Membership = m;
                    return u;
                };

                var users = dbConnection.Query<MeGrabUser, MeGrabMembership, MeGrabUser>(sqlStatement, userMap, parameters, null, true, "UserId", null, CommandType.Text);

                return users.SingleOrDefault();
            }
        }

        protected override string GetSingleAggregateRootQuerySqlStatementById(int id)
        {
            ISqlCriteriaExpression sqlCriteriaExpression = SqlQueryDialectProviderFactory.Default.SqlCriteriaExpression();
            sqlCriteriaExpression.Equals("webapp_users.UserId", id);

            return this.GetSingleAggregateRootQuerySqlStatementByCriteria(sqlCriteriaExpression);
        }

        protected override object GetSingleAggregateRootQueryParametersById(int id)
        {
            return new { id };
        }

        protected override string GetSingleAggregateRootQuerySqlStatementByCriteria(ISqlCriteriaExpression sqlCriteriaExpression)
        {
            ISqlBuilder sqlBuilder = SqlBuilder.Create();

            sqlBuilder.From("webapp_users")
                      .InnerJoin("webapp_membership", "UserId", "UserId")
                      .Where(sqlCriteriaExpression).Select("webapp_users.UserId AS Id",
                                                           "webapp_users.Name AS Name",
                                                           "webapp_membership.UserId AS UserId",
                                                           "webapp_membership.Email AS Email",
                                                           "webapp_membership.CellPhoneNo AS CellPhoneNo",
                                                           "webapp_membership.Password AS Password",
                                                           "webapp_membership.PasswordQuestion AS PasswordQuestion",
                                                           "webapp_membership.PasswordAnswer AS PasswordAnswer",
                                                           "webapp_membership.ConfirmationToken AS ConfirmationToken",
                                                           "webapp_membership.IsApproved AS IsApproved",
                                                           "webapp_membership.LastActivityDate AS LastActivityDate",
                                                           "webapp_membership.LastLoginDate AS LastLoginDate",
                                                           "webapp_membership.LastPasswordChangedDate AS LastPasswordChangedDate",
                                                           "webapp_membership.CreationDate AS CreationDate",
                                                           "webapp_membership.IsLockedOut AS IsLockedOut",
                                                           "webapp_membership.LastLockedOutDate AS LastLockedOutDate",
                                                           "webapp_membership.FailedPasswordAttemptCount AS FailedPasswordAttemptCount",
                                                           "webapp_membership.FailedPasswordAttemptWindowStart AS FailedPasswordAttemptWindowStart",
                                                           "webapp_membership.PasswordVerificationToken AS PasswordVerificationToken",
                                                           "webapp_membership.PasswordVerificationTokenExpirationDate AS PasswordVerificationTokenExpirationDate");

            return sqlBuilder.GetQuerySql();
        }

        protected override object GetSingleAggregateRootQueryParametersByCriteria(ISqlCriteriaExpression sqlCriteriaExpression)
        {
            return sqlCriteriaExpression.ParameterColumnValues;
        }

        protected override string GetAggregateRootListQuerySqlStatementByCriteria(ISqlCriteriaExpression sqlCriteriaExpression)
        {
            throw new NotImplementedException();
        }

        protected override object GetAggregateRootListQueryParametersByCriteria(ISqlCriteriaExpression sqlCriteriaExpression)
        {
            throw new NotImplementedException();
        }

        protected override string GetAggregateRootListPagingQuerySqlStatementByCriteria(ISqlCriteriaExpression sqlCriteriaExpression, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        protected override object GetAggregateRootListPagingQueryParametersByCriteria(ISqlCriteriaExpression sqlCriteriaExpression)
        {
            throw new NotImplementedException();
        }

        protected override string GetAggregateRootInsertSqlStatement()
        {
            throw new NotImplementedException();
        }

        protected override object GetAggregateRootInsertParameters(MeGrabUser aggregateRoot)
        {
            throw new NotImplementedException();
        }

        protected override string GetAggregateRootUpdateSqlStatement()
        {
            throw new NotImplementedException();
        }

        protected override object GetAggregateRootUpdateParameters(MeGrabUser aggregateRoot)
        {
            throw new NotImplementedException();
        }

        protected override string GetAggregateRootDeleteSqlStatement()
        {
            throw new NotImplementedException();
        }

        protected override object GetAggregateRootDeleteParameters(MeGrabUser aggregateRoot)
        {
            throw new NotImplementedException();
        }
    }
}
