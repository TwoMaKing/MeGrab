using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Core.Query
{
    [Serializable()]
    public class PagingResult<T> : IPagingResult<T>
    {
        private int? totalRecords;
        private int? totalPages;
        private int? pageNumber;
        private int? pageSzie;
        private IEnumerable<T> data;

        public PagingResult() { }

        public PagingResult(int? totalRecords, int? totalPages, int? pageNumber, int? pageSzie, IEnumerable<T> data)
        {
            this.totalRecords = totalRecords;
            this.totalPages = totalPages;
            this.pageNumber = pageNumber;
            this.pageSzie = pageSzie;
            this.data = data;
        }

        public int? TotalRecords
        {
            get 
            { 
                return this.totalRecords;
            }
            set
            {
                this.totalRecords = value;
            }
        }

        public int? TotalPages
        {
            get 
            { 
                return this.totalPages; 
            }
            set
            {
                this.totalPages = value;
            }
        }

        public int? PageNumber
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

        public int? PageSize
        {
            get 
            {
                return this.pageSzie;
            }
            set
            {
                this.pageSzie = value;
            }
        }

        public IEnumerable<T> Data
        {
            get 
            {
                return this.data;
            }
            set
            {
                this.data = value;
            }
        }
    }
}
