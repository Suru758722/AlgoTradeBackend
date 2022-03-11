using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SudhirTest.Model
{
    [DataContract]
    public class LoginResult
    {

        /// <summary>
        /// Gets or sets the token
        /// </summary>
        [DataMember(Name = "token")]
        public string token { get; set; }

        /// <summary>
        /// Gets or sets the userId
        /// </summary>
        [DataMember(Name = "userID")]
        public string userID { get; set; }

        /// <summary>
        /// Gets or sets the app version
        /// </summary>
        [DataMember(Name = "appVersion")]
        public string appVersion { get; set; }

    }
}
