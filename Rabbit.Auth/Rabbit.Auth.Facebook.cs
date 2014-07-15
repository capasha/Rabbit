using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerIOClient;
using System.Text.RegularExpressions;
using Rabbit.Auth;

namespace Rabbit
{
    public static class Facebook : Auth
    {
        public static Client Authenticate(string token)
        {
            return PlayerIO.QuickConnect.FacebookOAuthConnect("everybody-edits-su9rn58o40itdbnw69plyw", token, null);
        }
    }

}