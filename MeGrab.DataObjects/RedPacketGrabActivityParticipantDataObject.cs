using Eagle.Domain.Application;
using EmitMapper;
using MeGrab.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MeGrab.DataObjects
{

    [DataContract()]
    public class RedPacketGrabActivityParticipantDataObject
    {
        [DataMember()]
        public Guid RedPacketGrabActivity { get; set; }

        [DataMember()]
        public int UserId { get; set; }

        [DataMember()]
        public DateTime JoinedDateTime { get; set; }

        [DataMember()]
        public bool Quitted { get; set; }

    }
}
