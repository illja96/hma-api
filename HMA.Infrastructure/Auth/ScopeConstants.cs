﻿namespace HMA.Infrastructure.Auth
{
    public static class ScopeConstants
    {
        static ScopeConstants()
        {
            Scopes = new string[]
            {
                OpenId,
                Profile,
                Email
            };
        }

        public const string OpenId = "openid";
        public const string Profile = "profile";
        public const string Email = "email";

        public static readonly string[] Scopes;
    }
}
