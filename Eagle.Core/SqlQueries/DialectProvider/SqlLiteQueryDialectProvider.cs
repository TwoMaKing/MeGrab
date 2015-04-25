using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Core.SqlQueries.DialectProvider
{
    public class SqlLiteQueryDialectProvider : SqlQueryDialectProviderBase
    {
        private const char Parameter_Prefix = '?';

        public override string SelectLastInsertedRowAutoIDStatement
        {
            get { throw new NotImplementedException(); }
        }

        public override char ParameterPrefix
        {
            get { throw new NotImplementedException(); }
        }

        public override char ParameterLeftToken
        {
            get 
            { 
                return '['; 
            }
        }

        public override char ParameterRightToken
        {
            get 
            { 
                return ']'; 
            }
        }

        public override char WildCharToken
        {
            get 
            { 
                return '%'; 
            }
        }

        public override char WildSingleCharToken
        {
            get 
            {
                return '_';
            }
        }

        public override string CreateSelectRangeStatement(string tableName, string where, string orderBy, int topCount, int skipCount,
                                                  string identityColumn, bool identityColumnIsNumberOrSequence = true,
                                                  string groupBy = null, params string[] includedColumns)
        {
            throw new NotImplementedException();
        }
    }
}
