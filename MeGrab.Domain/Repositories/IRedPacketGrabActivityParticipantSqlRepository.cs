using Eagle.Domain.Repositories;
using MeGrab.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeGrab.Domain.Repositories
{
    public interface IRedPacketGrabActivityParticipantSqlRepository : ISqlRepository<RedPacketGrabActivityParticipant, Guid>
    {
        IEnumerable<MeGrabUser> GetParticipantedUsersByActivity(RedPacketGrabActivity activity);

        IEnumerable<RedPacketGrabActivity> GetParticipantedActivitiesByUser(MeGrabUser user);
    }
}
