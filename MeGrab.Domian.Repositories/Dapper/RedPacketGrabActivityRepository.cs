using Eagle.Domain.Repositories;
using Eagle.Repositories.Dapper;
using MeGrab.Domain.Models;
using MeGrab.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeGrab.Domain.Repositories.Dapper
{
    public class RedPacketGrabActivityRepository : DapperRepository<RedPacketGrabActivity, Guid>, IRedPacketGrabActivityRepository
    {
        public RedPacketGrabActivityRepository(IRepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        protected override string GetAggregateRootQuerySqlStatementById()
        {
            throw new NotImplementedException();
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
