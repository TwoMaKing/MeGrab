using Eagle.Core;
using Eagle.Domain;
using Eagle.Domain.Application;
using Eagle.Domain.Repositories;
using Eagle.Web.Caches;
using EmitMapper;
using MeGrab.DataObjects;
using MeGrab.Domain.Models;
using MeGrab.Domain.Repositories;
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
    public class RedPacketCommandService : ApplicationService, IRedPacketCommandService
    {
        private IRedPacketGrabActivityRepository redPacketRepository;

        public RedPacketCommandService(IRepositoryContext repositoryContext, 
                                       IRedPacketGrabActivityRepository redPacketRepository) : base(repositoryContext) 
        {
            this.redPacketRepository = redPacketRepository;
        }

        public void SaveRedPacketGrabActivity(RedPacketGrabActivityDataObject redPacketGrabActivity)
        {
            throw new NotImplementedException();
        }


    }
}
