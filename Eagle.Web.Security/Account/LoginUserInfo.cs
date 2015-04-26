using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Eagle.Web.Security
{
    [Serializable()]
    [DataContract()]
    public class LoginUserInfo
    {
        [DataMember()]
        public int Id { get; set; }

        [DataMember()]
        public string Name { get; set; }

        [DataMember()]
        public string Email { get; set; }

        [DataMember()]
        public string CellPhone { get; set; }

        [DataMember()]
        public string PassportToken { get; set; }

        [DataMember()]
        public string[] Roles { get; set; }

        [DataMember()]
        public DateTime LastLoginDateTime { get; set; }
    }
}
