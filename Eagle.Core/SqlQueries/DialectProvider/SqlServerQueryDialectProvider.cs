using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Eagle.Core.SqlQueries.DialectProvider
{
    public class SqlServerQueryDialectProvider : SqlQueryDialectProviderBase
    {
        private const char Parameter_Prefix = '@';

        public override string SelectLastInsertedRowAutoIDStatement
        {
            get 
            { 
                return "SELECT SCOPE_IDENTITY()"; 
            }
        }

        public override char ParameterPrefix
        {
            get 
            {
                return Parameter_Prefix; 
            }
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
