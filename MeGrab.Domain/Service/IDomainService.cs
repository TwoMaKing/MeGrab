using Eagle.Domain;
using MeGrab.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeGrab.Domain.Service
{
    public interface IDomainService
    {
        void JoinRedPacketGrabActivity(RedPacketGrabActivity activity, MeGrabUser user);
    }
}
