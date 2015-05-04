using Eagle.Core.Query;
using MeGrab.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MeGrab.DataObjects
{
    [DataContract()]
    public class DateTimeCriteriaRange 
    {
        [DataMember()]
        public DateTime? FromDateTime { get; set; }

        [DataMember()]
        public DateTime? ToDateTime { get; set; }
    }

    [DataContract()]
    public class TotalAmountCriteriaRange
    {
        [DataMember()]
        public decimal? FromTotalAmount { get; set; }

        [DataMember()]
        public decimal? ToTotalAmount { get; set; }
    }

    [DataContract()]
    public class RedPacketCountCriteriaRange
    {
        [DataMember()]
        public int? FromRedPacketCount { get; set; }

        [DataMember()]
        public int? ToRedPacketCount { get; set; }
    }

    [DataContract()]
    public class MemberLimitCriteriaRange
    {
        [DataMember()]
        public int? FromMemberLimit { get; set; }

        [DataMember()]
        public int? ToMemberLimit { get; set; }
    }

    [DataContract()]
    public class RedPacketGrabActivityQueryServiceRequest
    {
        public RedPacketGrabActivityQueryServiceRequest() { }

        private int pageNumber = 1;
        private int pageSize = 10;
        private bool cachePrefetching = true;

        [DataMember()]
        public DateTimeCriteriaRange StartDateTimeRange { get; set; }

        [DataMember()]
        public DateTimeCriteriaRange ExpireDateTimeRange { get; set; }

        [DataMember()]
        public DateTimeCriteriaRange DispatchDateTimeRange { get; set; }

        [DataMember()]
        public TotalAmountCriteriaRange TotalAmountRange { get; set; }

        [DataMember()]
        public RedPacketCountCriteriaRange RedPacketCountRange { get; set; }

        [DataMember()]
        public MemberLimitCriteriaRange MemberLimitRange { get; set; }

        [DataMember()]
        public DispatchMode? DispatchMode { get; set; }

        [DataMember()]
        public string OrderBy { get; set; }
        
        [DataMember()]
        public SortOrder SortOrder { get; set; }

        [DataMember()]
        public int PageNumber 
        { 
            get
            {
                return this.pageNumber;
            } 
            set
            {
                this.pageNumber = value;
            } 
        }

        [DataMember()]
        public int PageSize 
        { 
            get
            {
                return this.pageSize;
            } 
            set
            {
                this.pageSize = value;
            }
        }

        /// <summary>
        /// 是否预读缓存
        /// </summary>
        [DataMember()]
        public bool CachePrefetching
        {
            get 
            {
                return this.cachePrefetching;
            }
            set 
            {
                this.cachePrefetching = value;
            }
        }

    }


}
