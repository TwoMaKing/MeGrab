﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Core.SqlQueries.Criterias
{
    public class TextSqlCriteria : ISqlCriteria
    {
        private string sqlCriteria = string.Empty;

        public TextSqlCriteria() { }

        public TextSqlCriteria(string sqlCriteria)
        {
            this.SqlCriteria = sqlCriteria;
        }

        public string SqlCriteria
        {
            get 
            {
                return this.sqlCriteria;
            }
            set 
            {
                this.sqlCriteria = value;
            }
        }

        public string GetSqlCriteria()
        {
            return this.sqlCriteria;
        }
    }
}
