using Eagle.Domain;
using Eagle.Web.Security;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;

namespace MeGrab.Domain.Models
{
    [Alias("webapp_membership")]
    [TablePrimaryKey("UserId", ColumnName="UserId", AutoIncrement=false)]
    [Serializable()]
    public class MeGrabMembership : Membership
    {
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

            MeGrabMembership otherMembership = obj as MeGrabMembership;
            if (otherMembership == null)
            {
                return false;
            }

            return this.UserId.Equals(otherMembership.UserId);
        }

        public override int GetHashCode()
        {
            return this.UserId.GetHashCode();
        }
    }
}
