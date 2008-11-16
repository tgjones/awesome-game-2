using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SD.Shared
{
    /// <summary>
    /// Information about log in request
    /// </summary>
    public class LoginInfo
    {
        public bool LoginSuccessful;
        public Guid SessionKey;
        public string LoginFailReason;
    }
}
