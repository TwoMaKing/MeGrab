using Eagle.Domain;
using Eagle.Domain.Application;
using Eagle.Domain.Repositories;
using Eagle.Web.Caches;
using MeGrab.DataObjects;
using MeGrab.Domain.Models;
using MeGrab.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeGrab.Application
{
    public class RedPacketDispatchServiceImpl : ApplicationService, IRedPacketDispatchService
    {
        public RedPacketDispatchServiceImpl(IRepositoryContext repositoryContext) : base(repositoryContext) { }

        public void Dispatch(DispatchRequest dispatchRequest)
        {
            RedPacketGrabActivityDataObject redPacketGrabActivityDataObject = dispatchRequest.RedPacketGrabActivity;

            CacheFactory.GetCacheManager().AddItem<RedPacketGrabActivityDataObject>(redPacketGrabActivityDataObject.Id.ToString(),
                                                                                    redPacketGrabActivityDataObject);

            RedPacketGrabActivity redPacketGrabActivity = redPacketGrabActivityDataObject.MapTo();
            redPacketGrabActivity.Dispatch();
        }

        public DispatchResponse GetDispatchedRedPacketGrabActivity()
        {
            return null;
        }

    }
}
