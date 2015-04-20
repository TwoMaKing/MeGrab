using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eagle.Web.Security
{

    public class PassportAuthenticationTicket
    {
        /// <summary>
        /// Gets or sets the cookie path for the forms-authentication ticket.
        /// </summary>
        public string CookiePath { get; set; }

        /// <summary>
        ///  Gets or sets the local date and time at which the forms-authentication ticket expires.
        /// </summary>
        public DateTime Expiration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the forms-authentication ticket has expired.
        /// </summary>
        public bool Expired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the cookie that contains the forms-authentication
        ///  ticket information is persistent.
        /// </summary>
        public bool IsPersistent { get; set; }

        /// <summary>
        ///  Gets or sets the local date and time at which the forms-authentication ticket was
        ///  originally issued.
        /// </summary>
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// Gets or sets the user name associated with the forms-authentication ticket.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a user-specific string stored with the ticket.
        /// </summary>
        public string UserData { get; set; }

        /// <summary>
        /// Gets or sets the version number of the ticket.
        /// </summary>
        public int Version { get; set; }
    }
}