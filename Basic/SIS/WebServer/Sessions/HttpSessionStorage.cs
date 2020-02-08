﻿using System.Collections.Concurrent;
using SIS.HTTP.Sessions;

namespace SIS.MvcFramework.Sessions
{
    public class HttpSessionStorage
    {
        public const string SessionCookieKey = "Server_ID";

        private static readonly ConcurrentDictionary<string, IHttpSession> sessions = new ConcurrentDictionary<string, IHttpSession>();

        public static IHttpSession GetSession(string id)
        {
            return sessions.GetOrAdd(id, _ => new HttpSession(id));
        }

        public bool ContainsSession(string id)
        {
            return sessions.ContainsKey(id);
        }
    }
}
