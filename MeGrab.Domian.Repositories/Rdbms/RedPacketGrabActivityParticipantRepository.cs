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
using System.Data.Common;
using System.Linq;
using System.Text;

namespace MeGrab.Domain.Repositories.Sql
{
    public class RedPacketGrabActivityParticipantRepository : DapperSqlRepository<RedPacketGrabActivityParticipant, Guid>, IRedPacketGrabActivityParticipantSqlRepository
    {
        public RedPacketGrabActivityParticipantRepository(IRepositoryContext repositoryContext)
            : base(repositoryContext) { }

        protected override string GetSingleAggregateRootQuerySqlStatementById(Guid id)
        {
            throw new NotImplementedException();
        }

        protected override object GetSingleAggregateRootQueryParametersById(Guid id)
        {
            throw new NotImplementedException();
        }

        protected override string GetSingleAggregateRootQuerySqlStatementByCriteria(ISqlCriteriaExpression sqlCriteriaExpression)
        {
            throw new NotImplementedException();
        }

        protected override object GetSingleAggregateRootQueryParametersByCriteria(ISqlCriteriaExpression sqlCriteriaExpression)
        {
            throw new NotImplementedException();
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
            return @"INSERT INTO `redpacket_grab_activity_participants`
                    (`rpgap_id`,
                    `rpgap_rpga_id`,
                    `rpgap_user_id`,
                    `rpgap_joined_datetime`,
                    `rpgap_quitted`)
                    VALUES
                    (?Id,
                     ?RedPacketGrabActivityId,
                     ?UserId,
                     ?JoinedDateTime,
                     ?Quitted";
        }

        protected override object GetAggregateRootInsertParameters(RedPacketGrabActivityParticipant aggregateRoot)
        {
            return new
            {
                aggregateRoot.Id,
                aggregateRoot.RedPacketGrabActivityId,
                aggregateRoot.UserId,
                aggregateRoot.JoinedDateTime,
                aggregateRoot.Quitted
            };
        }

        protected override string GetAggregateRootUpdateSqlStatement()
        {
            return @"UPDATE 'redpacket_grab_activity_participants`
                    SET
                    `rpgap_rpga_id` = ?RedPacketGrabActivityId,
                    `rpgap_user_id` = ?UserId,
                    `rpgap_joined_datetime` = ?JoinedDateTime,
                    `rpgap_quitted` = ?Quitted
                    WHERE `rpgap_id` = ?Id;";
        }

        protected override object GetAggregateRootUpdateParameters(RedPacketGrabActivityParticipant aggregateRoot)
        {
            return new
            {
                aggregateRoot.RedPacketGrabActivityId,
                aggregateRoot.UserId,
                aggregateRoot.JoinedDateTime,
                aggregateRoot.Quitted,
                aggregateRoot.Id
            };
        }

        protected override string GetAggregateRootDeleteSqlStatement()
        {
            throw new NotImplementedException();
        }

        protected override object GetAggregateRootDeleteParameters(RedPacketGrabActivityParticipant aggregateRoot)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MeGrabUser> GetParticipantedUsersByActivity(RedPacketGrabActivity activity)
        {
            using(IDbConnection connection = this.DapperRepositoryContext.CreateConnection())
            {
                string querySql = @"select UserId, Name from webapp_users in UserId in 
                                    (select rpgap_user_id from redpacket_grab_activity_participants where rpgap_rpga_id = ?rpgap_rpga_id)";

                return connection.Query<MeGrabUser>(querySql, activity.Id);
            }
        }

        public IEnumerable<RedPacketGrabActivity> GetParticipantedActivitiesByUser(MeGrabUser user)
        {
            throw new NotImplementedException();
        }

    }
}
