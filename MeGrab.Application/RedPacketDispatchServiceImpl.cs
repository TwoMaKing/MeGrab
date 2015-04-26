using Eagle.Core;
using Eagle.Core.QuerySepcifications;
using Eagle.Domain;
using Eagle.Domain.Application;
using Eagle.Domain.Repositories;
using Eagle.Web.Caches;
using EmitMapper;
using MeGrab.DataObjects;
using MeGrab.Domain.Models;
using MeGrab.Domain.Repositories;
using MeGrab.Infrastructure;
using MeGrab.ServiceContracts;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeGrab.Application
{
    public class RedPacketDispatchServiceImpl : ApplicationService, IRedPacketDispatchService
    {
        private IMeGrabUserRepository userRepository;

        public RedPacketDispatchServiceImpl(IRepositoryContext repositoryContext, 
                                            IMeGrabUserRepository userRepository) 
            : base(repositoryContext) 
        {
            this.userRepository = userRepository;
        }

        public void Dispatch(DispatchRequest dispatchRequest)
        {
            RedPacketGrabActivityDataObject redPacketGrabActivityDataObject = dispatchRequest.RedPacketGrabActivity;
            RedPacketGrabActivity redPacketGrabActivity = redPacketGrabActivityDataObject.MapTo();

            //MeGrabUser currentDispatcher = userRepository.Find(new ExpressionSpecification<MeGrabUser>(
            //                                                   u => u.Name == dispatchRequest.DispatcherName));

            MeGrabUser currentDispatcher = new MeGrabUser();
            currentDispatcher.Id = GlobalApplication.CurrentLoginUser.Id;

            redPacketGrabActivity.Dispatch(currentDispatcher);
        }

    }
}
