using Eagle.Core;
using Eagle.Domain;
using Eagle.Domain.Application;
using Eagle.Domain.Repositories;
using Eagle.Web.Caches;
using EmitMapper;
using MeGrab.DataObjects;
using MeGrab.Domain.Models;
using MeGrab.Domain.Repositories;
using MeGrab.Domain.Service;
using MeGrab.ServiceContracts;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeGrab.Application
{
    public class RedPacketGrabActivityCommandServiceImpl : ApplicationService, IRedPacketGrabActivityCommandService
    {
        private IRedPacketGrabActivitySqlRepository redPacketRepository;
        private IMeGrabUserRepository userRepository;
        private IRedPacketGrabActivityParticipantSqlRepository redPacketActivityParticipantRepository;
        private IDomainService domainService;
        private ICacheManager cacheManager;

        public RedPacketGrabActivityCommandServiceImpl(IRepositoryContext repositoryContext, 
                                       IMeGrabUserRepository userRepository,
                                       IRedPacketGrabActivitySqlRepository redPacketRepository,
                                       IRedPacketGrabActivityParticipantSqlRepository redPacketActivityParticipantRepository,
                                       IDomainService domainService,
                                       ICacheManager cacheManager)
            : base(repositoryContext) 
        {
            this.redPacketRepository = redPacketRepository;
            this.userRepository = userRepository;
            this.redPacketActivityParticipantRepository = redPacketActivityParticipantRepository;
            this.domainService = domainService;
            this.cacheManager = cacheManager;
        }

        public IEnumerable<MeGrabUserDataObject> Join(Guid redPacketGrabActivityId, int userId)
        {
            RedPacketGrabActivity activity = this.cacheManager.Get<RedPacketGrabActivity>("RedPacketGrabActivity_" + redPacketGrabActivityId.ToString(), 
                () => redPacketRepository.FindByKey(redPacketGrabActivityId), 3600);

            MeGrabUser joinedUser = this.cacheManager.Get<MeGrabUser>("User_" + userId.ToString(),
                () => userRepository.FindByKey(userId), 3600);

            ObjectsMapper<MeGrabUser, MeGrabUserDataObject> mapper = 
                ObjectMapperManager.DefaultInstance.GetMapper<MeGrabUser, MeGrabUserDataObject>();

            domainService.JoinRedPacketGrabActivity(activity, joinedUser);

            Func<IEnumerable<MeGrabUserDataObject>> retrieveFunc = () =>
                {
                    IEnumerable<MeGrabUser> users =
                        redPacketActivityParticipantRepository.GetParticipantedUsersByActivity(activity);

                    List<MeGrabUserDataObject> userDTOList = new List<MeGrabUserDataObject>();

                    foreach (MeGrabUser userItem in users)
                    {
                        MeGrabUserDataObject userDTOItem = new MeGrabUserDataObject();
                        userDTOItem.MapFrom(userItem);

                        userDTOList.Add(userDTOItem);
                    }

                    return userDTOList;
                };

            string cacheKey = string.Format("RedPacketGrabActivity_{0}_Participants", redPacketGrabActivityId);

            IEnumerable<MeGrabUserDataObject> currentParticipants = this.cacheManager.Get<MeGrabUserDataObject>(cacheKey, retrieveFunc, 3600);

            List<MeGrabUserDataObject> currentParticipantsDTOList = currentParticipants.ToList();

            MeGrabUserDataObject currentParticipantDTO = new MeGrabUserDataObject();
            currentParticipantDTO.MapFrom(joinedUser);

            if (!currentParticipants.Contains(currentParticipantDTO))
            {
                currentParticipantsDTOList.Add(currentParticipantDTO);
            }

            return currentParticipantsDTOList;
        }
    }
}
