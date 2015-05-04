using Eagle.Core.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MeGrab.DataObjects
{

    [Serializable()]
    [DataContract()]
    public class RedPacketGrabActivityQueryServiceResponse
    {
        [DataMember()]
        public IPagingResult<RedPacketGrabActivityDataObject> RedPacketGrabActivities { get; set; }
    }

    [Serializable()]
    [DataContract()]
    public class RecentlyDispatchedRedPacketActivityResponse
    {
        [DataMember()]
        public DateTime LastDispatchedDateTime { get; set; }

        [DataMember()]
        public IEnumerable<RedPacketGrabActivityDataObject> RedPacketGrabActivities { get; set; }
    }
}
