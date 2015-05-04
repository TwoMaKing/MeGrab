using MeGrab.DataObjects;
using MeGrab.Domain.Models;
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

    public class RedPacketGrabActivityQueriesModel
    {
        private DateTime selectedStartDateTime = new DateTime(2015, 04, 27);
        private int[] selectedTotalAmountRange = new int[] { 1000, 2000 };
        private int pageNo = 1;
        private int pageSize = 10;

        public DateTime SelectedStartDateTime 
        {
            get 
            {
                return this.selectedStartDateTime;
            }
            set 
            {
                this.selectedStartDateTime = value;
            }
        }

        public DateTime? SelectedExpireDateTime { get; set; }

        public int[] SelectedTotalAmountRange 
        {
            get 
            {
                return this.selectedTotalAmountRange;
            }
            set 
            {
                this.selectedTotalAmountRange = value;
            }
        }

        public DispatchMode SelectedPlayModel { get; set; }

        public int? TotalRecords { get; set; }

        public int? TotalPages { get; set; }

        public int PageNo 
        {
            get 
            {
                return this.pageNo;
            }
            set 
            {
                this.pageNo = value;
            }
        }

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

        public IEnumerable<RedPacketGrabActivityDataObject> PagingRedPacketGrabActivities { get; set; }
    }



}