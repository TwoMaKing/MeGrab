using MeGrab.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeGrab.Web.Models
{
    public class RedPacketGrabActivityModel
    {
        private DateTime startDateTime = DateTime.Now.Date;

        public DateTime StartDateTime 
        {
            get 
            {
                return this.startDateTime;
            }
            set 
            {
                this.startDateTime = value;
            }
        }

        public IEnumerable<RedPacketGrabActivityDataObject> RedPacketGrabActivityList { get; set; }
    }

    public class PagingRedPacketGrabActivityModel
    {
        public int? TotalRecords { get; set; }

        public int? TotalPages { get; set; }

        public int? PageNo { get; set; }

        public int? PageSize { get; set; }

        public IEnumerable<RedPacketGrabActivityDataObject> PagingRedPacketGrabActivities { get; set; }
    }
}