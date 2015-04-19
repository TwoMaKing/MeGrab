using Eagle.Domain;
using Eagle.Domain.Events;
using MeGrab.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeGrab.Domain.Events
{
    public class RedPacketGrabActivityEvent : DomainEvent<Guid>
    {
        public RedPacketGrabActivityEvent() { }

        public RedPacketGrabActivityEvent(IEntity<Guid> source) : base(source) { }
    }
}
