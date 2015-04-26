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
        private IRedPacketGrabActivitySqlRepository redPacketRepository;

        public RedPacketCommandService(IRepositoryContext repositoryContext, 
                                       IRedPacketGrabActivitySqlRepository redPacketRepository) : base(repositoryContext) 
        {
            this.redPacketRepository = redPacketRepository;
        }

        public void AddNewRedPacketGrabActivity(RedPacketGrabActivityDataObject redPacketGrabActivity)
        {
            throw new NotImplementedException();
        }

        public void UpdateRedPacketGrabActivity(RedPacketGrabActivityDataObject redPacketGrabActivity)
        {
            throw new NotImplementedException();
        }

        public void DeleteRedPacketGrabActivity(RedPacketGrabActivityDataObject redPacketGrabActivity)
        {
            throw new NotImplementedException();
        }
    }
}
