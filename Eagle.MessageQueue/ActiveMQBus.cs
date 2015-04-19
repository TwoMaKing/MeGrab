using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using Eagle.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.MessageQueue.ActiveMQ
{
    public class ActiveMQBus<TMessage> : DisposableObject, IMessageQueueBus<TMessage> where TMessage : class
    {
        private const string queueNamePrefixKey = "MQ.";

        private string queueName = string.Empty;
        private volatile bool committed = true;
        private static readonly object lockObj = new object();
        private readonly Queue<TMessage> mockQueue = new Queue<TMessage>();

        private IConnectionFactory activeMQConnectionFactory;

        public ActiveMQBus(string brokerUri, string clientId, string queueName)
        {
            this.queueName = queueName;
            this.activeMQConnectionFactory = new ConnectionFactory(brokerUri, clientId);
        }

        private IConnection CreateActiveMQConnection()
        {
            return this.activeMQConnectionFactory.CreateConnection();
        }

        protected override void Dispose(bool disposing) { }

        public void Publish(TMessage message)
        {
            lock (lockObj)
            {
                this.mockQueue.Enqueue(message);
                this.committed = false;
            }
        }

        public void Publish(IEnumerable<TMessage> messages)
        {
            lock (lockObj)
            {
                messages.ToList().ForEach(m =>
                {
                    this.mockQueue.Enqueue(m);
                    this.committed = false;
                });
            }
        }

        public IEnumerable<TMessage> Subscribe(int batchSize)
        {
            return null;
        }

        public bool DistributedTransactionSupported
        {
            get 
            {
                return true;
            }
        }

        public bool Committed
        {
            get 
            {
                return this.committed;
            }
        }

        public void Commit()
        {
            if (mockQueue == null ||
                mockQueue.Count.Equals(0))
            {
                return;
            }

            lock (lockObj)
            {
                using (IConnection connection = this.CreateActiveMQConnection())
                {
                    using (ISession session = connection.CreateSession())
                    {
                        IMessageProducer producer = session.CreateProducer(new ActiveMQQueue(this.queueName));
                        while (this.mockQueue.Count > 0)
                        {
                            TMessage message = mockQueue.Dequeue();
                            IObjectMessage objectMessage = producer.CreateObjectMessage(message);
                            producer.Send(objectMessage, MsgDeliveryMode.NonPersistent, MsgPriority.High, TimeSpan.MinValue);
                        }
                    }
                }

                this.committed = true;
            }
        }

        public void Rollback()
        {
            this.committed = false;
        }

    }
}
