using MeGrab.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeGrab.Dispatcher.Models
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
}