using Eagle.Domain;
using Eagle.Web.Security;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;

namespace MeGrab.Domain.Models
{
    /// <summary>
    /// 代表用户的聚合对象
    /// </summary>
    [Alias("webapp_users")]
    [TablePrimaryKey("Id", ColumnName = "UserId", AutoIncrement = true, ReferencesType= typeof(MeGrabMembership))]
    [TableIgnoreColumns(new string[] { "Accounts" })]
    [Serializable()]
    public class MeGrabUser : User, IAggregateRoot
    {
        public bool Enabled { get; set; }

        [Reference()]
        public MeGrabMembership Membership
        {
            get;
            set;
        }

        public IEnumerable<Account> Accounts { get; set; }

        public void Enable()
        {
            this.Enabled = true;
        }

        public void Disable()
        {
            this.Enabled = false;
        }

        public void AddAccount(Account account) { }

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

            MeGrabUser otherUser = obj as MeGrabUser;
            if (otherUser == null)
            {
                return false;
            }

            return this.Id.Equals(otherUser.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
