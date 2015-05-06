using Eagle.Domain;
using Eagle.Domain.Application;
using MeGrab.Domain.Models;
using System;
using System.Runtime.Serialization;

namespace MeGrab.DataObjects
{
    /// <summary>
    /// 代表用户的数据传输对象
    /// </summary>
    [Serializable()]
    [DataContract()]
    public class MeGrabUserDataObject : DataTransferObjectBase<MeGrabUser>
    {
        [DataMember()]
        public string Name { get; set; }

        [DataMember()]
        public string Email { get; set; }

        [DataMember()]
        public string CellPhoneNo { get; set; }

        protected override void DoMapFrom(MeGrabUser domainModel)
        {
            this.Id = domainModel.Id;
            this.Name = domainModel.Name;
            if (domainModel.Membership != null)
            {
                this.Email = domainModel.Membership.Email;
                this.CellPhoneNo = domainModel.Membership.CellPhoneNo;
            }
        }

        protected override MeGrabUser DoMapTo()
        {
            MeGrabUser user = new MeGrabUser();
            user.Id = this.Id;
            user.Name = this.Name;
            user.Membership.Email = this.Email;
            user.Membership.CellPhoneNo = this.CellPhoneNo;

            return user;
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

            MeGrabUserDataObject other = obj as MeGrabUserDataObject;
            if (other == null)
            {
                return false;
            }

            return this.Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Utils.GetHashCode(this.Id);
        }
    }
}