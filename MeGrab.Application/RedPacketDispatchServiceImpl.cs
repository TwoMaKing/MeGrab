using Eagle.Core;
using Eagle.Domain;
using Eagle.Domain.Application;
using Eagle.Domain.Repositories;
using Eagle.Web.Caches;
using EmitMapper;
using MeGrab.DataObjects;
using MeGrab.Domain.Models;
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
    public class RedPacketDispatchServiceImpl : DisposableObject, IRedPacketDispatchService
    {
        public void Dispatch(DispatchRequest dispatchRequest)
        {
            RedPacketGrabActivityDataObject redPacketGrabActivityDataObject = dispatchRequest.RedPacketGrabActivity;

            RedPacketGrabActivity redPacketGrabActivity = redPacketGrabActivityDataObject.MapTo();
            redPacketGrabActivity.Dispatch();
        }

        protected override void Dispose(bool disposing)
        { }
    }
}
