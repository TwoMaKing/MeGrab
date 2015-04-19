using Eagle.Domain.Application;
using MeGrab.DataObjects;
using System;
using System.Collections.Generic;
using System.ServiceModel;


namespace MeGrab.ServiceContracts
{
    public interface IRedPacketCommandService : IApplicationServiceContract
    {
        void SaveRedPacketGrabActivity(RedPacketGrabActivityDataObject redPacketGrabActivity);

    }
}
