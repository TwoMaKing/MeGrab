using Eagle.Core;
using Eagle.Domain;
using Eagle.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Domain
{
    public class AggregateRoot<TEntityIdentityKey> : EntityBase<TEntityIdentityKey>, IAggregateRoot<TEntityIdentityKey>
    {
        protected void RaiseEvent<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : class, IEvent
        {
            IEnumerable<IEventHandler<TDomainEvent>> eventHandlers = ServiceLocator.Instance.ResolveAll<IEventHandler<TDomainEvent>>();

            if (eventHandlers != null &&
                eventHandlers.Count() > 0)
            {
                foreach (IEventHandler<TDomainEvent> eventHandler in eventHandlers)
                {
                    if (eventHandler.GetType().IsDefined(typeof(AsynchronouslyHandleAttribute), false))
                    {
                        Task.Factory.StartNew(() => eventHandler.Handle(domainEvent));
                    }
                    else
                    {
                        eventHandler.Handle(domainEvent);
                    }
                }
            }
        }

        protected void RaiseEvent<TDomainEvent>(TDomainEvent domainEvent, Action<TDomainEvent, bool, Exception> callback, TimeSpan? timeout = null)
                where TDomainEvent : class, IEvent
        {
            IEnumerable<IEventHandler<TDomainEvent>> eventHandlers = ServiceLocator.Instance.ResolveAll<IEventHandler<TDomainEvent>>();

            if (eventHandlers != null &&
                eventHandlers.Count() > 0)
            {
                List<Task> tasks = new List<Task>();

                try
                {
                    foreach (var handler in eventHandlers)
                    {
                        if (handler.GetType().IsDefined(typeof(AsynchronouslyHandleAttribute), false))
                        {
                            tasks.Add(Task.Factory.StartNew(() => handler.Handle(domainEvent)));
                        }
                        else
                        {
                            handler.Handle(domainEvent);
                        }
                    }

                    if (tasks.Count > 0)
                    {
                        if (timeout == null)
                        {
                            Task.WaitAll(tasks.ToArray());
                        }
                        else
                        {
                            Task.WaitAll(tasks.ToArray(), timeout.Value);
                        }
                    }

                    callback(domainEvent, true, null);
                }
                catch (Exception ex)
                {
                    callback(domainEvent, false, ex);
                }
            }
            else
            {
                callback(domainEvent, false, null);
            }
        }

    }
}
