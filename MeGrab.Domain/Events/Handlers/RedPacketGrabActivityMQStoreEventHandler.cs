using Eagle.Domain;
using Eagle.Domain.Events;
using Eagle.MessageQueue;
using Eagle.MessageQueue.ActiveMQ;
using MeGrab.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeGrab.Domain.Events.Handlers
{
    public class RedPacketGrabActivityMQStoringEventHandler : IDomainEventHandler<RedPacketGrabActivityEvent, Guid>
    {
        public void Handle(RedPacketGrabActivityEvent message)
        {
            // 进入 保存队列
            using (IMessageQueueBus<RedPacketGrabActivity> storingMQBus =
                   new ActiveMQBus<RedPacketGrabActivity>("tcp://localhost:61616", 
                                                          "MQ.StoringRedPacketActivity.ConnectionId", 
                                                          "MQ.StoringRedPacketActivity"))
            {
                storingMQBus.Publish((RedPacketGrabActivity)message.Source);
                storingMQBus.Commit();
            }
        }
    }
}
