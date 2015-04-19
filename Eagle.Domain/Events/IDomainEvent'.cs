using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Domain.Events
{
    /// <summary>
    /// The domain event data.
    /// </summary>
    public interface IDomainEvent<TEntityIdentityKey> : IEvent
    {
        /// <summary>
        /// The source which generate this event.
        /// </summary>
        IEntity<TEntityIdentityKey> Source { get; set; }

        long Version { get; set; }

        long Branch { get; set; }
    }
}
