using Eagle.Core;
using Eagle.Core.Application;
using Eagle.Core.Extensions;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using ServiceStack.Redis.Support.Locking;
using ServiceStack.Redis.Support.Queue.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Eagle.MessageQueue.Redis
{
    public class DistributedRedisMQBus<TMessage> : DisposableObject, IMessageQueueBus<TMessage> where TMessage : class
    {
        private const string queueNamePrefixKey = "MQ.";

        private string queueName = string.Empty;
        private volatile bool committed = true;
        private static readonly object lockObj = new object();
        private readonly Queue<TMessage> mockQueue = new Queue<TMessage>();

        public DistributedRedisMQBus()
            : this(queueNamePrefixKey + typeof(TMessage).Name)
        { }

        public DistributedRedisMQBus(string queueName)
        {
            this.queueName = queueName;
        }

        private RedisSequentialWorkQueue<TMessage> CreateRedisSequentialWorkQueue()
        {
            string writeServerList = AppRuntime.Instance.CurrentApplication.ConfigSource.Config.Redis.WriteHosts;
            string[] writeHosts = writeServerList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            string host = "127.0.0.1";
            int port = 6379;

            if (writeHosts != null &&
                writeHosts.Length > 0)
            {
                string[] hostPortPair = writeHosts[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (hostPortPair != null &&
                    hostPortPair.Length > 0)
                {
                    host = hostPortPair[0];
                    port = Convert.ToInt32(hostPortPair[1]);
                }
            }

            int maxWritePoolSize = AppRuntime.Instance.CurrentApplication.ConfigSource.Config.Redis.MaxWritePoolSize;
            int maxReadPoolSize = AppRuntime.Instance.CurrentApplication.ConfigSource.Config.Redis.MaxReadPoolSize;

            return new RedisSequentialWorkQueue<TMessage>(maxReadPoolSize, maxWritePoolSize, host, port, this.queueName, 1);
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
            using (var redisQueue = this.CreateRedisSequentialWorkQueue())
            {
                List<TMessage> messageList = new List<TMessage>();

                redisQueue.PrepareNextWorkItem();
                var batch = redisQueue.Dequeue(batchSize);

                for (int i = 0; i < batch.DequeueItems.Count; i++)
                {
                    messageList.Add(batch.DequeueItems[i]);
                }

                Thread.Sleep(500);

                redisQueue.HarvestZombies();

                for (int i = 0; i < batch.DequeueItems.Count; i++)
                {
                    batch.DoneProcessedWorkItem();
                }

                return messageList;
            }
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
                using (var redisQueue = this.CreateRedisSequentialWorkQueue())
                {
                    while (this.mockQueue.Count > 0)
                    {
                        TMessage message = mockQueue.Dequeue();
                        redisQueue.Enqueue(message.GetHashCode().ToString(), message);
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
