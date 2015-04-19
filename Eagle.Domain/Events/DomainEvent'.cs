using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Domain.Events
{
    public abstract class DomainEvent<TEntityIdentityKey> : IDomainEvent<TEntityIdentityKey>
    {
        private DateTime timestamp = DateTime.UtcNow;

        private IEntity<TEntityIdentityKey> source;

        public DomainEvent() { }

        public DomainEvent(IEntity<TEntityIdentityKey> source)
        {
            this.source = source;
        }

        public Guid Id
        {
            get;
            set;
        }

        public IEntity<TEntityIdentityKey> Source
        {
            get
            {
                return this.source;
            }
            set
            {
                this.source = value;
            }
        }

        public DateTime Timestamp
        {
            get
            {
                return this.timestamp;
            }
            set
            {
                this.timestamp = value;
            }
        }

        public long Version
        {
            get;
            set;
        }

        public long Branch
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj == null)
            {
                return false;
            }

            DomainEvent<TEntityIdentityKey> other = obj as DomainEvent<TEntityIdentityKey>;

            if ((object)other == (object)null)
            {
                return false;
            }

            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Utils.GetHashCode(this.Source.GetHashCode(),
                                     this.Branch.GetHashCode(),
                                     this.Id.GetHashCode(),
                                     this.Timestamp.GetHashCode(),
                                     this.Version.GetHashCode());
        }
    }

}
