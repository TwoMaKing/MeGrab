using Eagle.Domain;
using Eagle.Domain.Repositories;
using MeGrab.Domain.Models;
using MeGrab.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeGrab.Domain.Service
{
    public class DomainService : IDomainService
    {
        private IRepositoryContext repositoryContext;

        private IRedPacketGrabActivityParticipantSqlRepository redPacketGrabActivityParticipantRepository;
        
        public DomainService(IRepositoryContext repositoryContext,
                             IRedPacketGrabActivityParticipantSqlRepository redPacketGrabActivityParticipantRepository)
        {
            this.repositoryContext = repositoryContext;
            this.redPacketGrabActivityParticipantRepository = redPacketGrabActivityParticipantRepository;
        }

        public void JoinRedPacketGrabActivity(RedPacketGrabActivity activity, MeGrabUser user)
        {
            RedPacketGrabActivityParticipant participant = new RedPacketGrabActivityParticipant(activity.Id, user.Id, DateTime.Now);

            redPacketGrabActivityParticipantRepository.Add(participant);

            repositoryContext.Commit();
        }
    }
}
