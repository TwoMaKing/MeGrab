using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Core.SqlQueries.Criterias
{
    public interface ICompositeSqlCriteria : ISqlCriteria
    {
        ISqlCriteria Left { get; }

        ISqlCriteria Right { get; }
    }
}
