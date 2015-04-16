using Eagle.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.MessageQueue
{
    public interface IMessageQueueBus<TMessage> : IUnitOfWork, IDisposable where TMessage : class
    {
        /// <summary>
        /// 发布消息 
        /// </summary>
        void Publish(TMessage message);

        /// <summary>
        /// 发布消息
        /// </summary>
        void Publish(IEnumerable<TMessage> messages);

        /// <summary>
        /// 订阅消息返回 消息集合
        /// </summary>
        IEnumerable<TMessage> Subscribe(int batchSize);

    }
}
