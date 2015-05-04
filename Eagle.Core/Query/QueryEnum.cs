using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Eagle.Core.Query
{
    /// <summary>
    /// SQL 运算操作符
    /// </summary>
    [DataContract()]
    public enum Operator
    {
        [DataMember()]
        Equal,

        [DataMember()]
        NotEqual,

        [DataMember()]
        GreaterThan,

        [DataMember()]
        GreaterThanEqual,

        [DataMember()]
        LessThan,

        [DataMember()]
        LessThanEqual,

        [DataMember()]
        Contains,

        [DataMember()]
        StartsWith,

        [DataMember()]
        EndsWith,

        [DataMember()]
        In,

        [DataMember()]
        NotIn
    }


    /// <summary>
    /// Specifies how items in a list are sorted.
    /// </summary>
    [DataContract()]
    public enum SortOrder
    {
        /// <summary>
        /// The items are not sorted
        /// </summary>
        [DataMember()]
        [XmlEnum()]
        None = 0,

        /// <summary>
        /// The items are sorted in ascending order.
        /// </summary>
        [DataMember()]
        [XmlEnum()]
        Ascending = 1,

        /// <summary>
        /// The items are sorted in descending order.
        /// </summary>
        [DataMember()]
        [XmlEnum()]
        Descending = 2,
    }

}
