﻿using SIS.MvcFramework.Identity;
using System;

namespace SIS.MvcFramework.Attributes
{
    public class AuthorizeAttribute : Attribute
    {
        private readonly string authority;

        public AuthorizeAttribute(string authority = "authorized")
        {
            this.authority = authority;
        }

        public bool IsLoggedIn(Principal principal)
        {
            return principal != null;
        }

        public bool IsInAuthority(Principal principal)
        {
            if (!this.IsLoggedIn(principal))
            {
                return this.authority == "anonymous";
            }

            return this.authority == "authorized" || principal.Roles.Contains(this.authority.ToLower());
        }

    }
}
