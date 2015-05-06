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
using System.Linq;
using System.Text;

namespace MeGrab.Domain.Repositories.Sql
{
    public class RedPacketGrabActivityRepository : DapperSqlRepository<RedPacketGrabActivity, Guid>, IRedPacketGrabActivitySqlRepository
    {
        public RedPacketGrabActivityRepository(IRepositoryContext repositoryContext) : base(repositoryContext) { }

        protected override string GetSingleAggregateRootQuerySqlStatementByCriteria(ISqlCriteriaExpression sqlCriteriaExpression)
        {
            ISqlBuilder sqlBuilder = SqlBuilder.Create();

            sqlBuilder.From("redpacket_grab_activity")
                      .Where(sqlCriteriaExpression)
                      .Select("rpga_id AS Id",
                              "rpga_total_amount AS TotalAmount",
                              "rpga_redpacket_count AS RedPacketCount",
                              "rpga_play_mode AS Mode",
                              "rpga_limit_member AS MemberLimit",
                              "rpga_start_datetime AS StartDateTime",
                              "rpga_expire_datetime AS ExpireDateTime",
                              "rpga_message AS Message",
                              "rpga_dispatcher_id AS DispatcherId",
                              "rpga_dispatch_datetime AS DispatchDateTime",
                              "rpga_cancelled AS Cancelled",
                              "rpga_finished AS Finished",
                              "rpga_last_modified_datetime AS LastModifiedUserId",
                              "rpga_last_modified_user_id AS LastModifiedUserId");

            return sqlBuilder.GetQuerySql();
        }

        protected override object GetSingleAggregateRootQueryParametersByCriteria(ISqlCriteriaExpression sqlCriteriaExpression)
        {
            return sqlCriteriaExpression.ParameterColumnValues;
        }

        protected override string GetAggregateRootListQuerySqlStatementByCriteria(ISqlCriteriaExpression sqlCriteriaExpression)
        {
            ISqlBuilder sqlBuilder = new SqlBuilder(new MySqlQueryDialectProvider());

            sqlBuilder.From("redpacket_grab_activity")
                      .Where(sqlCriteriaExpression)
                      .Select("rpga_id AS Id",
                              "rpga_total_amount AS TotalAmount",
                              "rpga_redpacket_count AS RedPacketCount",
                              "rpga_play_mode AS Mode",
                              "rpga_limit_member AS MemberLimit",
                              "rpga_start_datetime AS StartDateTime",
                              "rpga_expire_datetime AS ExpireDateTime",
                              "rpga_message AS Message",
                              "rpga_dispatcher_id AS DispatcherId",
                              "rpga_dispatch_datetime AS DispatchDateTime",
                              "rpga_cancelled AS Cancelled",
                              "rpga_finished AS Finished",
                              "rpga_last_modified_datetime AS LastModifiedUserId",
                              "rpga_last_modified_user_id AS LastModifiedUserId");

            return sqlBuilder.GetQuerySql();
        }

        protected override object GetAggregateRootListQueryParametersByCriteria(ISqlCriteriaExpression sqlCriteriaExpression)
        {
            return sqlCriteriaExpression.ParameterColumnValues;
        }

        protected override string GetAggregateRootListPagingQuerySqlStatementByCriteria(ISqlCriteriaExpression sqlCriteriaExpression, int pageNumber, int pageSize)
        {
            ISqlBuilder sqlBuilder = new SqlBuilder(new MySqlQueryDialectProvider());

            sqlBuilder.From("redpacket_grab_activity")
                      .Where(sqlCriteriaExpression)
                      .Select("rpga_id AS Id",
                              "rpga_total_amount AS TotalAmount",
                              "rpga_redpacket_count AS RedPacketCount",
                              "rpga_play_mode AS Mode",
                              "rpga_limit_member AS MemberLimit",
                              "rpga_start_datetime AS StartDateTime",
                              "rpga_expire_datetime AS ExpireDateTime",
                              "rpga_message AS Message",
                              "rpga_dispatcher_id AS DispatcherId",
                              "rpga_dispatch_datetime AS DispatchDateTime",
                              "rpga_cancelled AS Cancelled",
                              "rpga_finished AS Finished",
                              "rpga_last_modified_datetime AS LastModifiedUserId",
                              "rpga_last_modified_user_id AS LastModifiedUserId")
                       .Page(pageNumber, pageSize, "rpga_id");

            return sqlBuilder.GetQuerySql();
        }

        protected override object GetAggregateRootListPagingQueryParametersByCriteria(ISqlCriteriaExpression sqlCriteriaExpression)
        {
            return sqlCriteriaExpression.ParameterColumnValues;
        }

        protected override string GetAggregateRootInsertSqlStatement()
        {

            return @"INSERT INTO `redpacket_grab_activity`
                                (`rpga_id`,
                                 `rpga_total_amount`,
                                 `rpga_redpacket_count`,
                                 `rpga_play_mode`,
                                 `rpga_limit_member`,
                                 `rpga_start_datetime`,
                                 `rpga_expire_datetime`,
                                 `rpga_message`,
                                 `rpga_dispatcher_id`,
                                 `rpga_dispatch_datetime`,
                                 `rpga_cancelled`,
                                 `rpga_finished`,
                                 `rpga_last_modified_datetime`,
                                 `rpga_last_modified_user_id`)
                                 VALUES
                                ( ?Id,
                                  ?TotalAmount,
                                  ?RedPacketCount,
                                  ?Mode,
                                  ?MemberLimit,
                                  ?StartDateTime,
                                  ?ExpireDateTime,
                                  ?Message,
                                  ?DispatcherId,
                                  ?DispatchDateTime,
                                  ?Cancelled,
                                  ?Finished,
                                  null,
                                  ?LastModifiedUserId);";
        }

        protected override object GetAggregateRootInsertParameters(RedPacketGrabActivity aggregateRoot)
        {
            return new
            {
                aggregateRoot.Id,
                aggregateRoot.TotalAmount,
                aggregateRoot.RedPacketCount,
                aggregateRoot.Mode,
                aggregateRoot.MemberLimit,
                aggregateRoot.StartDateTime,
                aggregateRoot.ExpireDateTime,
                aggregateRoot.Message,
                aggregateRoot.DispatcherId,
                aggregateRoot.DispatchDateTime,
                aggregateRoot.Cancelled,
                aggregateRoot.Finished,
                aggregateRoot.LastModifiedUserId
            };
        }

        protected override string GetAggregateRootUpdateSqlStatement()
        {
            throw new NotImplementedException();
        }

        protected override object GetAggregateRootUpdateParameters(RedPacketGrabActivity aggregateRoot)
        {
            throw new NotImplementedException();
        }

        protected override string GetAggregateRootDeleteSqlStatement()
        {
            throw new NotImplementedException();
        }

        protected override object GetAggregateRootDeleteParameters(RedPacketGrabActivity aggregateRoot)
        {
            throw new NotImplementedException();
        }
    }
}
