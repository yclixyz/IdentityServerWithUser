using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer4.UserStorage
{
    public class UserClaims
    {
        public UserClaims(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        public string Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public string IdentityUserId { get; set; }

        public IdentityUser IdentityUser { get; set; }
    }
}
