using Eagle.Domain.Application;
using MeGrab.DataObjects;
using System;
using System.Collections.Generic;
using System.ServiceModel;


namespace MeGrab.ServiceContracts
{
    public interface IRedPacketCommandService : IApplicationServiceContract
    {
        void AddNewRedPacketGrabActivity(RedPacketGrabActivityDataObject redPacketGrabActivity);

        void UpdateRedPacketGrabActivity(RedPacketGrabActivityDataObject redPacketGrabActivity);

        void DeleteRedPacketGrabActivity(RedPacketGrabActivityDataObject redPacketGrabActivity);
    }
}
