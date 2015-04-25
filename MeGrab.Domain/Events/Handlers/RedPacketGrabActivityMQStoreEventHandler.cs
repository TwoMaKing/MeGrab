using Eagle.Core.Generators;
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
                                                          "MQ.StoringRedPacketActivity"))
            {
                RedPacketGrabActivity activity = (RedPacketGrabActivity)message.Source;

                for (int i = 1; i < 100; i++)
                {
                    RedPacketGrabActivity nextActivity = new RedPacketGrabActivity();
                    nextActivity.Id = (Guid)SequenceGenerator.Instance.Next;

                    nextActivity.RedPacketCount = activity.RedPacketCount;
                    nextActivity.MemberLimit = activity.MemberLimit;
                    nextActivity.DispatcherId = 5;
                    nextActivity.DispatchDateTime = DateTime.UtcNow.AddMinutes(i);
                    nextActivity.TotalAmount = activity.TotalAmount + i;
                    nextActivity.StartDateTime = activity.StartDateTime.Value.AddHours(i);
                    nextActivity.ExpireDateTime = activity.ExpireDateTime.AddHours(i);
                    nextActivity.Message = "新年快乐，恭喜发财 红包: " + i.ToString();

                    storingMQBus.Publish(nextActivity);
                }                    
                
                storingMQBus.Commit();
            }
        }
    }
}
