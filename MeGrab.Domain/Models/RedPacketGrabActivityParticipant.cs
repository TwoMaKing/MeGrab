using Eagle.Core;
using Eagle.Core.Generators;
using Eagle.Domain;
using MeGrab.Domain.Events;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeGrab.Domain.Models
{
    public class RedPacketGrabActivityParticipant : AggregateRoot<Guid>
    {
        public RedPacketGrabActivityParticipant() : base()
        { }

        public RedPacketGrabActivityParticipant(Guid redPacketGrabActivityId, int userId)
        {
            this.RedPacketGrabActivityId = redPacketGrabActivityId;
            this.UserId = userId;
        }

        public Guid RedPacketGrabActivityId { get; set; }

        public int UserId { get; set; }

        public DateTime JoinedDateTime { get; set; }

        public bool Quitted { get; set; }
    }
}
